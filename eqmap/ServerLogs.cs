using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eqmap
{
    public partial class ServerLogs : Form
    {
        public ServerLogs()
        {
            InitializeComponent();
            List<string> files = GetLogFiles("honeytree-eqemu-server-1", "server/logs");
            foreach (string file in files)
            {
                ListViewItem item = new ListViewItem(file.Split('\t')[0]);
                item.SubItems.Add(file.Split('\t')[1]);
                listView1.Items.Add(item);
            }            
        }

        private List<string> GetLogFiles(string container, string folder)
        {
            List<string> logFiles = new List<string>(); 
                        
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"exec {container} ls -l {folder}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                List<FileEntry> fileEntries = new  List<FileEntry>();

                foreach (string line in output.Split('\n'))
                {
                    if (line.EndsWith(".log"))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Date parts
                        string month = parts[5];
                        string day = parts[6];
                        string time = parts[7];
                        string fileName = string.Join(" ", parts, 8, parts.Length - 8);
                        string dateStr = $"{month} {day} {DateTime.Now.Year} {time}";
                        DateTime modified = DateTime.ParseExact(dateStr, "MMM d yyyy HH:mm", CultureInfo.InvariantCulture);
                        fileEntries.Add(new FileEntry
                        {
                            FileName = fileName,
                            LastModified = modified
                        });
                    }
                }
                fileEntries.Sort((a, b) => b.LastModified.CompareTo(a.LastModified));
                foreach (var entry in fileEntries)
                {
                    var item = new ListViewItem(entry.FileName);
                    item.SubItems.Add(entry.LastModified.ToString("yyyy-MM-dd HH:mm"));
                    listView1.Items.Add(item);
                }                
            }
            else
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }

            return logFiles;
        }

        private List<string> GetLogFile(string container, string file)
        {
            List<string> events = new List<string>();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"exec {container} cat {file}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                foreach (string line in output.Split('\n'))
                {
                    events.Add(line.Trim());               
                }
            }
            else
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }

            return events;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (listView1.SelectedItems.Count > 0)
            {
                string file = listView1.SelectedItems[0].Text.Trim();
                List<string> events = GetLogFile("\"honeytree-eqemu-server-1\"", $"server/logs/{file}");
                foreach (string line in events)
                {
                    listBox2.Items.Add(line.Trim());
                }
            }
        }
        class FileEntry {
            public string FileName { get; set; }
            public DateTime LastModified { get; set; }
        }
    }    
}
