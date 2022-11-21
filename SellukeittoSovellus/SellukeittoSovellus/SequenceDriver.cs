using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SellukeittoSovellus
{
    class SequenceDriver
    {

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
            Console.WriteLine("Running sequence one...");
            // TODO
            return 0;
        }

        private int RunSequenceTwo()
        {
            Console.WriteLine("Running sequence two...");
            // TODO
            return 0;
        }

        private int RunSequenceThree()
        {
            Console.WriteLine("Running sequence three...");
            // TODO
            return 0;
        }

        private int RunSequenceFour()
        {
            Console.WriteLine("Running sequence four...");
            // TODO

            return 0;
        }

        private int RunSequenceFive()
        {
            Console.WriteLine("Running sequence five...");
            // TODO
            return 0;
        }
    

    }
}
