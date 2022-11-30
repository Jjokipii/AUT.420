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

        private const string SEQUENCE1_STRING = "Vaihe 1";
        private const string SEQUENCE2_STRING = "Vaihe 2";
        private const string SEQUENCE3_STRING = "Vaihe 3";
        private const string SEQUENCE4_STRING = "Vaihe 4";
        private const string SEQUENCE5_STRING = "Vaihe 5";

        public bool sequence_finished = false;

        Logger logger = new Logger();

        public string current_sequence_state;

        public SequenceDriver()
        {
            logger.WriteLog("Sequence Driver started.");

            new Thread(() => // Start control thread
            {
                Thread.CurrentThread.IsBackground = true;
                RunSequence();
            }).Start();

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
        
        private int RunSequenceOne()
        {
            logger.WriteLog("Running sequence one...");
            current_sequence_state = SEQUENCE1_STRING;
            // TODO
            return 0;
        }

        private int RunSequenceTwo()
        {
            logger.WriteLog("Running sequence two...");
            current_sequence_state = SEQUENCE2_STRING;
            // TODO
            return 0;
        }

        private int RunSequenceThree()
        {
            logger.WriteLog("Running sequence three...");
            current_sequence_state = SEQUENCE3_STRING;
            // TODO
            return 0;
        }

        private int RunSequenceFour()
        {
            logger.WriteLog("Running sequence four...");
            current_sequence_state = SEQUENCE4_STRING;
            // TODO

            return 0;
        }

        private int RunSequenceFive()
        {
            logger.WriteLog("Running sequence five...");
            current_sequence_state = SEQUENCE5_STRING;
            // TODO
            return 0;
        }
    

    }
}
