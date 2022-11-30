using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SellukeittoSovellus
{
    class SequenceDriver
    {

        Logger logger = new Logger();

        public SequenceDriver()
        {
            // TODO

        }
        
        public int RunSequence(int state)
        {
            switch (state)
            {
                case 1:
                    RunSequenceOne();
                    break;

                case 2:
                    RunSequenceTwo();
                    break;

                case 3:
                    RunSequenceThree();
                    break;

                case 4:
                    RunSequenceFour();
                    break;

                case 5:
                    RunSequenceFive();
                    break;

                default:
                    return -1;
     
            }

            return 0;
        }
        
        private int RunSequenceOne()
        {
            logger.WriteLog("Running sequence one...");
            // TODO
            return 0;
        }

        private int RunSequenceTwo()
        {
            logger.WriteLog("Running sequence two...");
            // TODO
            return 0;
        }

        private int RunSequenceThree()
        {
            logger.WriteLog("Running sequence three...");
            // TODO
            return 0;
        }

        private int RunSequenceFour()
        {
            logger.WriteLog("Running sequence four...");
            // TODO

            return 0;
        }

        private int RunSequenceFive()
        {
            logger.WriteLog("Running sequence five...");
            // TODO
            return 0;
        }
    

    }
}
