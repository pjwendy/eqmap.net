using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    public class Log
    {
        private Main main;
        public Log(Main main)
        {
            this.main = main;
        }
        public void Info(string message)
        {
            main.Info($"{message}", Main.LogSource.lua);
        }
    }
}
