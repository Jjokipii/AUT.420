using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;


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

        #endregion


        #region OBJECTS

        Logger logger = new Logger();

        #endregion


        //#############################################


        public SequenceDriver()
        {
            logger.WriteLog("Sequence Driver started.");
            sequencedrivethread = new Thread(() => // Start control thread
            {
                Thread.CurrentThread.IsBackground = true;
                RunSequence();
            });
            sequencedrivethread.Start();
        }
        
        private void RunSequence()
        {
            RunSequenceOne();
            Thread.Sleep(5000);
            RunSequenceTwo();
            Thread.Sleep(5000);
            RunSequenceThree();
            Thread.Sleep(5000);
            RunSequenceFour();
            Thread.Sleep(5000);
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
            logger.WriteLog("Running sequence one...");
            current_sequence_state = SEQUENCE1_STRING;
        }

        private void RunSequenceTwo()
        {
            logger.WriteLog("Running sequence two...");
            current_sequence_state = SEQUENCE2_STRING;
        }

        private void RunSequenceThree()
        {
            logger.WriteLog("Running sequence three...");
            current_sequence_state = SEQUENCE3_STRING;
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
    }
}
