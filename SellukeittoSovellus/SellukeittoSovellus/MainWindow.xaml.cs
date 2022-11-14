﻿using System;
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

        // System states
        private const int STATE_FAILSAFE = 0;
        private const int STATE_DISCONNECTED = 1;
        private const int STATE_IDLE = 2;
        private const int STATE_RUNNING = 3;

        // Tank parameters
        private const int TANK_MAX_VALUE = 300;
        private const int TANK_MIN_VALUE = 0;

        #endregion

        private const int THREAD_DELAY_MS = 10;

        private int State;

        public MainWindow()
        {
            // Set internal variables
            State = STATE_DISCONNECTED;

            InitializeComponent();

            // Init static UI elemets
            InitUI();

            // Update system controls 
            UpdateControl();

            // Start main thread
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ControlThread();
            }).Start();
        }

        private void ControlThread()
        {
            try
            {
                while (true) // TODO close
                {
                    Console.WriteLine("{0} ControlThread tick", DateTime.Now.ToString("hh:mm:ss"));
                    
                    // TODO get values from process

                    // TODO update values to UI

                    // TODO implement system control


                    
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
                    Console.WriteLine("ERROR: UpdateControl() switch default statement called");
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

        private void InitUI()
        {
            
            // Set progress bar MAX values
            progressBar_T100.Maximum = TANK_MAX_VALUE;
            progressBar_T200.Maximum = TANK_MAX_VALUE;
            progressBar_T300_pressure.Maximum = TANK_MAX_VALUE;
            progressBar_T300_temperature.Maximum = TANK_MAX_VALUE;
            progressBar_T400.Maximum = TANK_MAX_VALUE;

            // Set progress bar MIN values
            progressBar_T100.Minimum = TANK_MIN_VALUE;
            progressBar_T200.Minimum = TANK_MIN_VALUE;
            progressBar_T300_pressure.Minimum = TANK_MIN_VALUE;
            progressBar_T300_temperature.Minimum = TANK_MIN_VALUE;
            progressBar_T400.Minimum = TANK_MIN_VALUE;
        }

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
