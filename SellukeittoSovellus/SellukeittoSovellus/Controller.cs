using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace SellukeittoSovellus
{
    class Controller
    {
        // Constants
        #region CONSTANTS
        public const string PARAMETER_TEXTFILE_PATH = "\\default_parameter_values.txt"
        #endregion

        // Class variables
        #region CLASS VARIABLES
        public int default_Cooking_time;
        public int default_Cooking_temperature;
        public int default_Cooking_pressure;
        public int default_Impregnation_time;

        public int Cooking_time;
        public int Cooking_temperature;
        public int Cooking_pressure;
        public int Impregnation_time;
        #endregion

        public Controller()
        {
            readDefaultParametersFromFile();
            // TODO
        }

        private void readDefaultParametersFromFile()
        {
            try
            {
                string basedirectory = AppDomain.CurrentDomain.BaseDirectory;
                for (int i = 0; i <= 3; i++)
                {
                    basedirectory = Directory.GetParent(basedirectory).ToString();
                }
                string parameters_filepath = basedirectory + PARAMETER_TEXTFILE_PATH;
                Console.WriteLine(parameters_filepath);
                string[] lines = File.ReadAllLines(parameters_filepath);
                string[] parameters;

                foreach (string line in lines)
                {
                    parameters = line.Split('=');
                    declareDefaultParameters(parameters);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                Console.WriteLine("Using zeros as a default values instead.");

                default_Cooking_time = 0;
                default_Cooking_temperature = 0;
                default_Cooking_pressure = 0;
                default_Impregnation_time = 0;
            }
        }

        private void declareDefaultParameters(string[] parameters)
        {
            if (parameters[0] == "default_Cooking_time")
            {
                default_Cooking_time = Int32.Parse(parameters[1]);
                Console.WriteLine("Default cooking time loaded.");
            }
            else if (parameters[0] == "default_Cooking_temperature")
            {
                default_Cooking_temperature = Int32.Parse(parameters[1]);
                Console.WriteLine("Default cooking temperature loaded.");
            }
            else if (parameters[0] == "default_Cooking_pressure")
            {
                default_Cooking_pressure = Int32.Parse(parameters[1]);
                Console.WriteLine("Default cooking pressure loaded.");
            }
            else if (parameters[0] == "default_Impregnation_time")
            {
                default_Impregnation_time = Int32.Parse(parameters[1]);
                Console.WriteLine("Default impregnation time loaded.");
            }
        }


        
        // TODO implement thread.
        
            
    }
}
