using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace SellukeittoSovellus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CONSTANTS

        private const int STATE_FAILSAFE = 0;
        private const int STATE_DISCONNECTED = 1;
        private const int STATE_IDLE = 2;
        private const int STATE_RUNNING = 3;

        private const int THREAD_DELAY_MS = 10;

        #endregion

        private int State;

        public MainWindow()
        {
            State = STATE_DISCONNECTED;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ControlThread();
            }).Start();
            
            InitializeComponent();
        }

        public void ControlThread()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("ControlThread tick");
                    // TODO get values

                    UpdateControl(State);
                    
                    Thread.Sleep(THREAD_DELAY_MS);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
        public void UpdateControl(int state)
        {
            switch (state)
            {
                case STATE_FAILSAFE:
                    break;

                case STATE_DISCONNECTED:
                    break;

                case STATE_IDLE:
                    break;

                case STATE_RUNNING:
                    break;

                default:
                    break;
            }
        }


        // TODO UpdateValues

        private void button_start_process_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
