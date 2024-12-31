namespace eqviewer
{
    partial class Viewer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelBottom = new Panel();
            panelMain = new Panel();
            pictureBoxMap = new PictureBox();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).BeginInit();
            SuspendLayout();
            // 
            // panelBottom
            // 
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 410);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(800, 40);
            panelBottom.TabIndex = 2;
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.Controls.Add(pictureBoxMap);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(800, 410);
            panelMain.TabIndex = 3;
            // 
            // pictureBoxMap
            // 
            pictureBoxMap.Location = new Point(0, 0);
            pictureBoxMap.Name = "pictureBoxMap";
            pictureBoxMap.Size = new Size(100, 50);
            pictureBoxMap.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxMap.TabIndex = 0;
            pictureBoxMap.TabStop = false;
            // 
            // Viewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelMain);
            Controls.Add(panelBottom);
            Name = "Viewer";
            Text = "Viewer";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBottom;
        private Panel panelMain;
        private PictureBox pictureBoxMap;
    }
}
