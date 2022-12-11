using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Tuni.MppOpcUaClientLib;


namespace SellukeittoSovellus
{
    class SequenceDriver
    {

        #region CONSTANTS

        private const string SEQUENCE1_STRING = "Vaihe 1";
        private const string SEQUENCE2_STRING = "Vaihe 2";
        private const string SEQUENCE3_STRING = "Vaihe 3";
        private const string SEQUENCE4_STRING = "Vaihe 4";
        private const string SEQUENCE5_STRING = "Vaihe 5";

        #endregion


        #region CLASS VARIABLES

        public string current_sequence_state;
        public bool sequence_finished = false;
        private Thread sequencedrivethread;
        public bool sequence_error = false;

        public double Cooking_time;
        public double Cooking_temperature;
        public double Cooking_pressure;
        public double Impregnation_time;

        #endregion


        #region OBJECTS

        Logger logger = new Logger();
        MppClient mClient;
        ProcessClient mProcessClient;

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

            sequencedrivethread = new Thread(() => // Start control thread
            {
                Thread.CurrentThread.IsBackground = true;
                RunSequence();
            });
            sequencedrivethread.Start();
        }

        private void RunSequence()
        {
            // Compulsory flags by Salmari.
            mClient.SetOnOffItem("P100_P200_PRESET", true);

            RunSequenceOne();

            RunSequenceTwo();

            RunSequenceThree();

            RunSequenceFour();

            RunSequenceFive();

            sequence_finished = true;

            logger.WriteLog("Sequence finished succesfully!");
            logger.WriteLog("");
        }

        public void StopSequence()
        {
            logger.WriteLog("Stopping SequenceDrive process at stage " + current_sequence_state);
            sequencedrivethread.Abort();
        }

        private void RunSequenceOne()
        {
            // Impregnation process
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

            Thread.Sleep((int)(Impregnation_time)); // TODO TIME

            EM2_OP2();
            EM5_OP3();
            EM3_OP6();

            EM3_OP8();
        }

        private void RunSequenceTwo()
        {
            logger.WriteLog("Running sequence two...");
            current_sequence_state = SEQUENCE2_STRING;

            EM3_OP2();
            EM5_OP1();
            EM4_OP1();

            //int limit = mProcessClient.mData.LI200;
            while (mProcessClient.mData.LI400 > 31) // TODO START VALUE
            {
                Thread.Sleep(10);
            }

            EM3_OP6();
            EM5_OP3();
            EM4_OP2();
        }

        private void RunSequenceThree()
        {
            logger.WriteLog("Running sequence three...");
            current_sequence_state = SEQUENCE3_STRING;

            EM3_OP3();
            EM1_OP2();

            while (mProcessClient.mData.LI400 < 90) // TODO START VALUE
            {
                Thread.Sleep(10);
            }

            EM3_OP6();
            EM1_OP4();
        }

        private void RunSequenceFour()
        {
            logger.WriteLog("Running sequence four...");
            current_sequence_state = SEQUENCE4_STRING;
        }

        private void RunSequenceFive()
        {
            logger.WriteLog("Running sequence five...");
            current_sequence_state = SEQUENCE5_STRING;
        }

        private void EM1_OP1()
        {
            mClient.SetValveOpening("V102", 100);
            mClient.SetOnOffItem("V304", true);
            mClient.SetPumpControl("P100", 100);
            mClient.SetOnOffItem("E100", true);
        }

        private void EM1_OP2()
        {
            mClient.SetValveOpening("V102", 100);
            mClient.SetOnOffItem("V304", true);
            mClient.SetPumpControl("P100", 100);
        }

        private void EM1_OP3()
        {
            mClient.SetValveOpening("V102", 0);
            mClient.SetOnOffItem("V304", false);
            mClient.SetPumpControl("P100", 0);
            mClient.SetOnOffItem("E100", false);
        }
        private void EM1_OP4()
        {
            mClient.SetValveOpening("V102", 0);
            mClient.SetOnOffItem("V304", false);
            mClient.SetPumpControl("P100", 0);
        }
        private void EM2_OP1()
        {
            mClient.SetOnOffItem("V201", true);
        }
        private void EM2_OP2()
        {
            mClient.SetOnOffItem("V201", false);
        }

        private void EM3_OP1()
        {
            mClient.SetValveOpening("V104", 0);
            mClient.SetOnOffItem("V204", false);
            mClient.SetOnOffItem("V401", false);
        }

        private void EM3_OP2()
        {
            mClient.SetOnOffItem("V204", true);
            mClient.SetOnOffItem("V301", true);
        }

        private void EM3_OP3()
        {
            mClient.SetOnOffItem("V301", true);
            mClient.SetOnOffItem("V401", true);
        }

        private void EM3_OP4()
        {
            mClient.SetValveOpening("V104", 100);
            mClient.SetOnOffItem("V301", true);
        }
        private void EM3_OP5()
        {
            mClient.SetOnOffItem("V204", true);
            mClient.SetOnOffItem("V302", true);
        }

        private void EM3_OP6()
        {
            mClient.SetValveOpening("V104", 0);
            mClient.SetOnOffItem("V204", false);
            mClient.SetOnOffItem("V301", false);
            mClient.SetOnOffItem("V401", false);
        }

        private void EM3_OP7()
        {
            mClient.SetOnOffItem("V302", false);
            mClient.SetValveOpening("V204", 0);

        }

        private void EM3_OP8()
        {
            mClient.SetOnOffItem("V204", true);
            // Value 1000 is Td and defined in the customer requirements.
            Thread.Sleep(1000);
            mClient.SetOnOffItem("V204", false);
        }

        private void EM4_OP1()
        {
            mClient.SetOnOffItem("V404", true);
        }

        private void EM4_OP2()
        {
            mClient.SetOnOffItem("V404", false);
        }

        private void EM5_OP1()
        {
            mClient.SetOnOffItem("V303", true);
            mClient.SetPumpControl("P200", 100);
        }

        private void EM5_OP2()
        {
            mClient.SetOnOffItem("V103", true);
            mClient.SetOnOffItem("V303", true);
            mClient.SetPumpControl("P200", 100);
        }

        private void EM5_OP3()
        {
            mClient.SetOnOffItem("V303", false);
            mClient.SetPumpControl("P200", 0);
        }

        private void EM5_OP4()
        {
            mClient.SetOnOffItem("V103", false);
            mClient.SetOnOffItem("V303", false);
            mClient.SetPumpControl("P200", 0);
        }
    }
}