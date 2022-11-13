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

            UpdateControl();
        }

        private void ControlThread()
        {
            try
            {
                while (true)
                {
                    //Console.WriteLine("ControlThread tick");
                    // TODO get values

                    // TODO update UI


                    
                    Thread.Sleep(THREAD_DELAY_MS);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
        private void UpdateControl()
        {
            switch (State)
            {
                case STATE_FAILSAFE:
                    button_connect.IsEnabled = false;
                    button_start_process.IsEnabled = false;
                    button_interrupt_process.IsEnabled = false;
                    button_set_parameters.IsEnabled = false;
                    button_reset_parameters.IsEnabled = false;
                    UpdateParameterControls(false);
                    break;
                case STATE_DISCONNECTED:
                    button_connect.IsEnabled = true;
                    button_start_process.IsEnabled = false;
                    button_interrupt_process.IsEnabled = false;
                    button_set_parameters.IsEnabled = false;
                    button_reset_parameters.IsEnabled = false;
                    UpdateParameterControls(false);
                    break;
                case STATE_IDLE:
                    button_connect.IsEnabled = false;
                    button_start_process.IsEnabled = true;
                    button_interrupt_process.IsEnabled = false;
                    button_set_parameters.IsEnabled = true;
                    button_reset_parameters.IsEnabled = true;
                    UpdateParameterControls(true);
                    break;
                case STATE_RUNNING:
                    button_connect.IsEnabled = false;
                    button_start_process.IsEnabled = false;
                    button_interrupt_process.IsEnabled = true;
                    button_set_parameters.IsEnabled = false;
                    button_reset_parameters.IsEnabled = false;
                    UpdateParameterControls(false);
                    break;
                default:
                    Console.WriteLine("UpdateControl ERROR");
                    break;
            }
        }

        private void UpdateParameterControls(bool isEnabled)
        {
            // Update slider controls status
            slider_cooking_pressure.IsEnabled = isEnabled;
            slider_cooking_temperature.IsEnabled = isEnabled;
            slider_cooking_time.IsEnabled = isEnabled;
            slider_saturation_time.IsEnabled = isEnabled;
            
            // Update textbox slider status
            textBox_cooking_pressure.IsEnabled = isEnabled;
            textBox_cooking_temperature.IsEnabled = isEnabled;
            textBox_cooking_time.IsEnabled = isEnabled;
            textBox_saturation_time.IsEnabled = isEnabled;
        }

        // TODO UpdateValues

        private void button_start_process_Click(object sender, RoutedEventArgs e)
        {
            // TODO

            State = STATE_RUNNING;

            UpdateControl();
        }

        private void button_interrupt_process_Click(object sender, RoutedEventArgs e)
        {
            // TODO

            State = STATE_FAILSAFE;

            UpdateControl();
        }

        private void button_connect_Click(object sender, RoutedEventArgs e)
        {
            // TODO

            State = STATE_IDLE;

            UpdateControl();
        }

        private void button_set_parameters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_reset_parameters_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
