using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Threading;


namespace SellukeittoSovellus
{
    class Controller
    {
        #region CONSTANTS

        public const int STATE_FAILSAFE = 0;
        public const int STATE_DISCONNECTED = 1;
        public const int STATE_IDLE = 2;
        public const int STATE_RUNNING = 3;

        #endregion

        #region CLASS VARIABLES
        
        public double Cooking_time;
        public double Cooking_temperature;
        public double Cooking_pressure;
        public double Impregnation_time;

        public int State; // Controller state

        ProcessClient mProcessClient = new ProcessClient();

        #endregion

        #region CONFIGURABLE PARAMETERS

        private const int THREAD_DELAY_MS = 10;
        

        #endregion

        public Controller()
        {
            State = STATE_DISCONNECTED;

            new Thread(() => // Start control thread
            {
                Thread.CurrentThread.IsBackground = true;
                ControlThread();
            }).Start();
        }

        private void ControlThread()
        {
            try
            {
                while (true) // TODO close
                {
                    //Console.WriteLine("{0} ControlThread tick", DateTime.Now.ToString("hh:mm:ss"));

                    // TODO check connection status
                    
                    // Get values

                    // Update controls

                    // Update values

                    switch (State)
                    {
                        case STATE_FAILSAFE:
                            
                            break;
                        case STATE_DISCONNECTED:
                            
                            break;
                        case STATE_IDLE:
                            
                            break;
                        case STATE_RUNNING:

                            break;
                        default:
                            Console.WriteLine("ERROR: UpdateValues() switch default statement called");
                            break;
                    }

                    Thread.Sleep(THREAD_DELAY_MS);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
            
    }
}
