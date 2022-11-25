using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SellukeittoSovellus
{
    class ProcessClient
    {

        private Tuni.MppOpcUaClientLib.ConnectionParamsHolder mConnectionParamsHolder;
        private Tuni.MppOpcUaClientLib.MppClient mMppClient;

        public int connectionState = DISCONNECTED;

        public const int CONNECTED = 1;
        public const int DISCONNECTED = 0;

        private const string CLIENT_URL = "opc.tcp://127.0.0.1:8087";

        public ProcessClient()
        {
            initializeProcessClient();

            startProcessConnection(); 
        }

        private void initializeProcessClient()
        {
            mConnectionParamsHolder = new Tuni.MppOpcUaClientLib.ConnectionParamsHolder(CLIENT_URL); // init parameter holder
        }

        public void startProcessConnection()
        {
            try
            {
                mMppClient = new Tuni.MppOpcUaClientLib.MppClient(mConnectionParamsHolder);
                mMppClient.Init();

                connectionState = CONNECTED;
            }
            catch (Exception ex)
            {
                // TODO reset params

                Console.WriteLine(ex.Message);
                connectionState = DISCONNECTED;
            }
        }

    }
}
