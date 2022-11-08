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
            // TODO
            return 0;
        }

        private int RunSequenceTwo()
        {
            // TODO
            return 0;
        }

        private int RunSequenceThree()
        {
            // TODO
            return 0;
        }

        private int RunSequenceFour()
        {
            // TODO

            return 0;
        }

        private int RunSequenceFive()
        {
            // TODO
            return 0;
        }
    

    }
}
