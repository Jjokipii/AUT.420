using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellukeittoSovellus {

    public class Logger
    {
        #region CONSTANTS

        public const string DEFAULT_LOGFILEPATH = "\\logs\\log.txt";

        #endregion

        public string logPath;

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
            logPath = basedirectory + DEFAULT_LOGFILEPATH;
            Console.WriteLine(logPath);
        }

        public void WriteLog(string message)
        {
            Console.WriteLine(message);
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(DateTime.Now + " : " + message);
            }
        }
    }
}

