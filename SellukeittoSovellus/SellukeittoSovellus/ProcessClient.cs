using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuni.MppOpcUaClientLib;

namespace SellukeittoSovellus
{
    public struct Data
    {
        public int LI100;
        public int LI200;
        public int PI300;
        public double TI300;
        public int LI400;
    }
    
    class ProcessClient
    {
        const string CLIENT_URL = "opc.tcp://127.0.0.1:8087";

        public bool mConnectionState = DISCONNECTED;

        const bool CONNECTED = true;
        const bool DISCONNECTED = false;

        // OPC
        ConnectionParamsHolder mConnectionParamsHolder;
        MppClient mMppClient;

        //DATA
        
        public Data mData = new Data();

        public ProcessClient()
        {
            mData = new Data();
            
            mConnectionParamsHolder = new ConnectionParamsHolder(CLIENT_URL);
            mMppClient = new MppClient(mConnectionParamsHolder);

            mMppClient.ConnectionStatus += new MppClient.ConnectionStatusEventHandler(ConnectionStatus);
            mMppClient.ProcessItemsChanged += new MppClient.ProcessItemsChangedEventHandler(ProcessItemsChanged);

            mMppClient.Init();

            addSubscriptions(); // TODO Muualle

            mConnectionState = CONNECTED;
        }

        private static void ConnectionStatus(object source, ConnectionStatusEventArgs args)
        {
            Console.WriteLine("Connection event" + args.StatusInfo.FullStatusString);

            // TODO 
        }

        private void ProcessItemsChanged(object source, ProcessItemChangedEventArgs args)
        {
            foreach(KeyValuePair<String, MppValue> item in args.ChangedItems)
            {
                switch (item.Key)
                {
                    case "LI100":
                        Console.WriteLine("LI100 set: " + item.Value.GetValue());
                        mData.LI100 = (int)item.Value.GetValue();
                        break;
                    case "LI200":
                        Console.WriteLine("LI200 set: " + item.Value.GetValue());
                        mData.LI200 = (int)item.Value.GetValue();
                        break;
                    case "PI300":
                        Console.WriteLine("PI300 set: " + item.Value.GetValue());
                        mData.PI300 = (int)item.Value.GetValue();
                        break;
                    case "TI300":
                        Console.WriteLine("TI300 set: " + item.Value.GetValue());
                        mData.TI300 = (double)item.Value.GetValue();
                        break;
                    case "LI400":
                        Console.WriteLine("LI400 set: " + item.Value.GetValue());
                        mData.LI400 = (int)item.Value.GetValue();
                        break;
                    default:
                        Console.WriteLine("ERROR: ProcessItemsChanged item " + item.Key + " not handeled" );
                        break;
                }
            }

        }

        private void addSubscriptions()
        {
            // Tanks
            mMppClient.AddToSubscription("LI100");
            mMppClient.AddToSubscription("LI200");
            mMppClient.AddToSubscription("PI300");
            mMppClient.AddToSubscription("TI300");
            mMppClient.AddToSubscription("LI400");

            //Valves

            //Pumps
        }

        public void startProcessConnection()
        {

        }

    }
}
