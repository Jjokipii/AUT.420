using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuni.MppOpcUaClientLib;

namespace SellukeittoSovellus
{
    public struct Data
    {
        public double TI100;
        public double LI200;
        public double PI300;
        public double TI300;
        public double LI400;
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
            //Console.WriteLine("##################################" + args.ChangedItems);
            foreach(KeyValuePair<String, MppValue> item in args.ChangedItems)
            {
                switch (item.Key)
                {
                    case "TI100":
                        mData.TI100 = (double) item.Value.GetValue();
                        break;
                    case "LI200":
                        mData.LI200 = (double)item.Value.GetValue();
                        break;
                    case "PI300":
                        mData.PI300 = (double)item.Value.GetValue();
                        break;
                    case "TI300":
                        mData.TI300 = (double)item.Value.GetValue();
                        break;
                    case "LI400":
                        mData.LI400 = (double)item.Value.GetValue();
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
            mMppClient.AddToSubscription("TI100");
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
