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
        
        public int Cooking_time;
        public int Cooking_temperature;
        public int Cooking_pressure;
        public int Impregnation_time;

        // For the state-machine
        public int State;

        #endregion

        #region CONFIGURABLE PARAMETERS

        private const int THREAD_DELAY_MS = 10;
        private const string CLIENT_URL = "opc.tcp://127.0.0.1:8087";

        #endregion

        #region OPC

        private Tuni.MppOpcUaClientLib.ConnectionParamsHolder mConnectionParamsHolder;
        private Tuni.MppOpcUaClientLib.MppClient mMppClient;

        #endregion

        public Controller()
        {
            State = STATE_DISCONNECTED;

            // mConnectionParamsHolder = new Tuni.MppOpcUaClientLib.ConnectionParamsHolder(CLIENT_URL);

            // CreateProcessConnection(); // Try to establish process connection

            new Thread(() => // Start control thread
            {
                Thread.CurrentThread.IsBackground = true;
                ControlThread();
            }).Start();
        }

        public void CreateProcessConnection()
        {
            try
            {
                mMppClient = new Tuni.MppOpcUaClientLib.MppClient(mConnectionParamsHolder);
                mMppClient.Init();

                State = STATE_IDLE;
            }
            catch (Exception ex)
            {
                // TODO reset params
                
                Console.WriteLine(ex.Message);
                State = STATE_DISCONNECTED;
            }
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

        
        
        // TODO implement thread.
        
            
    }
}
