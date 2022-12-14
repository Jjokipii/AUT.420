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

        public bool LSplus300;
        public bool LSminus300;
    }
    
    class ProcessClient
    {

        #region CONSTANTS

        private const bool CONNECTED = true;
        private const bool DISCONNECTED = false;
        private const string CLIENT_URL = "opc.tcp://127.0.0.1:8087";

        #endregion


        #region OBJECTS

        // OPC
        public MppClient mMppClient;
        ConnectionParamsHolder mConnectionParamsHolder;

        Logger logger = new Logger();

        #endregion


        #region VARIABLES

        public bool mConnectionState = DISCONNECTED;
        public Data mData = new Data();

        #endregion


        //###########################

        public ProcessClient()
        {
            mConnectionParamsHolder = new ConnectionParamsHolder(CLIENT_URL);
            ConnectOPCUA();
        }

        public bool ConnectOPCUA()
        {
            try
            {
                mMppClient = new MppClient(mConnectionParamsHolder);

                mMppClient.ConnectionStatus += new MppClient.ConnectionStatusEventHandler(ConnectionStatus);
                mMppClient.ProcessItemsChanged += new MppClient.ProcessItemsChangedEventHandler(ProcessItemsChanged);

                mMppClient.Init();

                addSubscriptions();

                mConnectionState = CONNECTED;

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);

                return false;
            }
        }

        private bool addSubscriptions()
        {
            try
            {
                // Tanks
                mMppClient.AddToSubscription("LI100");
                mMppClient.AddToSubscription("LI200");
                mMppClient.AddToSubscription("PI300");
                mMppClient.AddToSubscription("TI300");
                mMppClient.AddToSubscription("LI400");

                //Sensor
                mMppClient.AddToSubscription("LS+300");
                mMppClient.AddToSubscription("LS-300");
            
                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);

                return false;
            }
        }


        #region EVENTS

        private void ConnectionStatus(object source, ConnectionStatusEventArgs args)
        {
            try
            {
                mConnectionState = (args.StatusInfo.FullStatusString == "Connected");
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
            }
        }

        private void ProcessItemsChanged(object source, ProcessItemChangedEventArgs args)
        {
            try
            {
                foreach (KeyValuePair<String, MppValue> item in args.ChangedItems)
                {
                    switch (item.Key)
                    {
                        case "LI100":
                            mData.LI100 = (int)item.Value.GetValue();
                            break;
                        case "LI200":
                            mData.LI200 = (int)item.Value.GetValue();
                            break;
                        case "PI300":
                            mData.PI300 = (int)item.Value.GetValue();
                            break;
                        case "TI300":
                            mData.TI300 = (double)item.Value.GetValue();
                            break;
                        case "LI400":
                            mData.LI400 = (int)item.Value.GetValue();
                            break;
                        case "LS+300":
                            mData.LSplus300 = (bool)item.Value.GetValue();
                            break;
                        case "LS-300":
                            mData.LSminus300 = (bool)item.Value.GetValue();
                            break;
                        default:
                            logger.WriteLog("ERROR: ProcessItemsChanged item " + item.Key + " not handeled");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.Message);
            }
        }

        #endregion

    }
}
