using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellukeittoSovellus {
    /// <summary>
    /// Class used for logging messages to program logfile
    /// </summary>
    public class Logger
    {
        private string default_logfilepath = "\\logs\\log_" + System.DateTime.Today.ToString().Split(' ')[0] + ".txt";

        private string log_path;

        /// <summary>
        /// Constructor for logger class
        /// </summary>
        public Logger()
        {
            InitLogPath();
        }

        private void InitLogPath()
        {
            string basedirectory = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i <= 3; i++) // TODO modular
            {
                basedirectory = Directory.GetParent(basedirectory).ToString();
            }
            log_path = basedirectory + default_logfilepath;
            Console.WriteLine(log_path);
        }

        /// <summary>
        /// Method for writing messages to log
        /// </summary>
        /// <param name="message">Message to be written to log</param>
        public void WriteLog(string message)
        {
            Console.WriteLine(message);
            using (StreamWriter writer = new StreamWriter(log_path, true))
            {
                writer.WriteLine(DateTime.Now + " : " + message);
            }
        }
    }
}

