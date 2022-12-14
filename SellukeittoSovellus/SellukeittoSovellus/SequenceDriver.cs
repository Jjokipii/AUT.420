using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Tuni.MppOpcUaClientLib;
using System.Diagnostics;


namespace SellukeittoSovellus
{
    class SequenceDriver
    {

        #region CONSTANTS

        private const string SEQUENCE1_STRING = "VAIHE 1";
        private const string SEQUENCE2_STRING = "VAIHE 2";
        private const string SEQUENCE3_STRING = "VAIHE 3";
        private const string SEQUENCE4_STRING = "VAIHE 4";
        private const string SEQUENCE5_STRING = "VAIHE 5";

        #endregion


        #region CLASS VARIABLES

        public string current_sequence_state;
        public bool sequence_finished = false;
        public bool sequence_error = false;

        private double Cooking_time;
        private double Cooking_temperature;
        private double Cooking_pressure;
        private double Impregnation_time;

        private double V104controlValue = 100;

        #endregion


        #region OBJECTS

        private Logger logger = new Logger();

        private MppClient mClient;

        private ProcessClient mProcessClient;

        private Thread sequencedrivethread;

        #endregion


        //#############################################


        public SequenceDriver(double cooktime, double cooktemp, double cookpres, double imprtime, ProcessClient initializedProcessClient)
        {
            logger.WriteLog("Sequence Driver started.");

            this.Cooking_time = cooktime;
            this.Cooking_temperature = cooktemp;
            this.Cooking_pressure = cookpres;
            this.Impregnation_time = imprtime;
            this.mClient = initializedProcessClient.mMppClient;
            this.mProcessClient = initializedProcessClient;

            // Thread that handles sequence logic
            sequencedrivethread = new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true;
                RunSequence();
            });
            sequencedrivethread.Start();
        }

        private void RunSequence()
        {
            // Compulsory flag for process pumps to start
            mClient.SetOnOffItem("P100_P200_PRESET", true);

            RunSequenceOne();

            RunSequenceTwo();

            RunSequenceThree();

            RunSequenceFour();

            RunSequenceFive();

            sequence_finished = true;

            logger.WriteLog("Sequence finished succesfully!\n");
        }

        public bool LockProcess()
        {
            try
            {
                // Valves
                mProcessClient.mMppClient.SetValveOpening("V102", 0);
                mProcessClient.mMppClient.SetOnOffItem("V103", false);
                mProcessClient.mMppClient.SetValveOpening("V104", 0);
                mProcessClient.mMppClient.SetOnOffItem("V201", false);
                mProcessClient.mMppClient.SetOnOffItem("V204", false);
                mProcessClient.mMppClient.SetOnOffItem("V301", false);
                mProcessClient.mMppClient.SetOnOffItem("V302", false);
                mProcessClient.mMppClient.SetOnOffItem("V303", false);
                mProcessClient.mMppClient.SetOnOffItem("V304", false);
                mProcessClient.mMppClient.SetOnOffItem("V401", false);
                mProcessClient.mMppClient.SetOnOffItem("V404", false);

                // Pumps
                mProcessClient.mMppClient.SetPumpControl("P100", 0);
                mProcessClient.mMppClient.SetPumpControl("P200", 0);

                // Heater
                mProcessClient.mMppClient.SetOnOffItem("E100", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        public bool StopSequence()
        {
            try
            {
                logger.WriteLog("Stopping SequenceDrive process at stage " + current_sequence_state);
                sequencedrivethread.Abort();

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }


        #region MAIN SEQUENCES

        private bool RunSequenceOne()
        {
            try
            {
                logger.WriteLog("Running sequence one...");
                current_sequence_state = SEQUENCE1_STRING;

                EM2_OP1();
                EM5_OP1();
                EM3_OP2();

                while (!mProcessClient.mData.LSplus300) 
                {
                    Thread.Sleep(10);
                }

                EM3_OP1();

                Thread.Sleep((int)(Impregnation_time * 1000));

                EM2_OP2();
                EM5_OP3();
                EM3_OP6();

                EM3_OP8();

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool RunSequenceTwo()
        {
            try
            {
                logger.WriteLog("Running sequence two...");
                current_sequence_state = SEQUENCE2_STRING;

                EM3_OP2();
                EM5_OP1();
                EM4_OP1();

                while (mProcessClient.mData.LI400 > 27)
                {
                    Thread.Sleep(10);
                }

                EM3_OP6();
                EM5_OP3();
                EM4_OP2();

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool RunSequenceThree()
        {
            try
            {
                logger.WriteLog("Running sequence three...");
                current_sequence_state = SEQUENCE3_STRING;

                EM3_OP3();
                EM1_OP2();

                while (mProcessClient.mData.LI400 < 90)
                {
                    Thread.Sleep(10);
                }

                EM3_OP6();
                EM1_OP4();

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool RunSequenceFour()
        {
            try
            {
                logger.WriteLog("Running sequence four...");
                current_sequence_state = SEQUENCE4_STRING;

                EM3_OP4();
                EM1_OP1();

                while (mProcessClient.mData.TI300 < Cooking_temperature)
                {
                    Thread.Sleep(10);
                }

                EM3_OP1();
                EM1_OP2();

                // Drive cooking values up
                while (mProcessClient.mData.PI300 < (int)Cooking_pressure || mProcessClient.mData.TI300 < Cooking_temperature)
                {
                    U1_OP1_2();
                }

                // Maintaint cooking values for cooking time
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while(stopwatch.ElapsedMilliseconds < Cooking_time * 1000)
                {
                    Console.WriteLine(mProcessClient.mData.PI300);
                    sequence_error = (mProcessClient.mData.PI300 < Cooking_pressure - 10 || mProcessClient.mData.PI300 > Cooking_pressure + 10);
                    sequence_error = (mProcessClient.mData.TI300 < Cooking_temperature - 0.3 || mProcessClient.mData.TI300 > Cooking_temperature + 0.3);
                    U1_OP1_2();
                }

                EM3_OP6();
                EM1_OP4();

                EM3_OP8();
            
                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool RunSequenceFive()
        {
            try
            {
                logger.WriteLog("Running sequence five...");
                current_sequence_state = SEQUENCE5_STRING;

                EM5_OP2();
                EM3_OP5();

                while (mProcessClient.mData.LSminus300)
                {
                    Thread.Sleep(10);
                }

                EM5_OP4();
                EM3_OP7();

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        #endregion


        #region MINI SEQUENCE

        private bool U1_OP1_2()
        {
            try
            {
                V104controlValue = (V104controlValue -( 0.001*(Cooking_pressure - mProcessClient.mData.PI300)));

                if (V104controlValue > 100) { V104controlValue = 100; }
                if (V104controlValue < 0) { V104controlValue = 0; }

                mClient.SetValveOpening("V104", (int)V104controlValue);

                bool E100controlValue = mProcessClient.mData.TI300 < Cooking_temperature;
                mClient.SetOnOffItem("E100", E100controlValue);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM1_OP1()
        {
            try
            {
                mClient.SetValveOpening("V102", 100);
                mClient.SetOnOffItem("V304", true);
                mClient.SetPumpControl("P100", 100);
                mClient.SetOnOffItem("E100", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM1_OP2()
        {
            try
            {
                mClient.SetValveOpening("V102", 100);
                mClient.SetOnOffItem("V304", true);
                mClient.SetPumpControl("P100", 100);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM1_OP3()
        {
            try
            {
                mClient.SetValveOpening("V102", 0);
                mClient.SetOnOffItem("V304", false);
                mClient.SetPumpControl("P100", 0);
                mClient.SetOnOffItem("E100", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM1_OP4()
        {
            try
            {
                mClient.SetValveOpening("V102", 0);
                mClient.SetOnOffItem("V304", false);
                mClient.SetPumpControl("P100", 0);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM2_OP1()
        {
            try
            {
                mClient.SetOnOffItem("V201", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM2_OP2()
        {
            try
            {
                mClient.SetOnOffItem("V201", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP1()
        {
            try
            {
                mClient.SetValveOpening("V104", 0);
                mClient.SetOnOffItem("V204", false);
                mClient.SetOnOffItem("V401", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP2()
        {
            try
            {
                mClient.SetOnOffItem("V204", true);
                mClient.SetOnOffItem("V301", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP3()
        {
            try
            {
                mClient.SetOnOffItem("V301", true);
                mClient.SetOnOffItem("V401", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP4()
        {
            try
            {
                mClient.SetValveOpening("V104", 100);
                mClient.SetOnOffItem("V301", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP5()
        {
            try
            {
                mClient.SetOnOffItem("V204", true);
                mClient.SetOnOffItem("V302", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP6()
        {
            try
            {
                mClient.SetValveOpening("V104", 0);
                mClient.SetOnOffItem("V204", false);
                mClient.SetOnOffItem("V301", false);
                mClient.SetOnOffItem("V401", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP7()
        {
            try
            {
                mClient.SetOnOffItem("V302", false);
                mClient.SetOnOffItem("V204", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM3_OP8()
        {
            try
            {
                mClient.SetOnOffItem("V204", true);
                // Value 1000 is Td and defined in the customer requirements.
                Thread.Sleep(1000);
                mClient.SetOnOffItem("V204", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM4_OP1()
        {
            try
            {
                mClient.SetOnOffItem("V404", true);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM4_OP2()
        {
            try
            {
                mClient.SetOnOffItem("V404", false);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM5_OP1()
        {
            try
            {
                mClient.SetOnOffItem("V303", true);
                mClient.SetPumpControl("P200", 100);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM5_OP2()
        {
            try
            {
                mClient.SetOnOffItem("V103", true);
                mClient.SetOnOffItem("V303", true);
                mClient.SetPumpControl("P200", 100);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM5_OP3()
        {
            try
            {
                mClient.SetOnOffItem("V303", false);
                mClient.SetPumpControl("P200", 0);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        private bool EM5_OP4()
        {
            try
            {
                mClient.SetOnOffItem("V103", false);
                mClient.SetOnOffItem("V303", false);
                mClient.SetPumpControl("P200", 0);

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
                return false;
            }
        }

        #endregion

    }
}