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

        // System states
        private const int STATE_FAILSAFE = 0;
        private const int STATE_DISCONNECTED = 1;
        private const int STATE_IDLE = 2;
        private const int STATE_RUNNING = 3;
        private const string STATE_FAILSAFE_STRING = "Vikaturvallinen";
        private const string STATE_DISCONNECTED_STRING = "Verkkoyhteydetön";
        private const string STATE_IDLE_STRING = "Odottaa";
        private const string STATE_RUNNING_STRING = "Prosessi käynnissä";
        private const string STATE_CONNECTED_STRING = "Yhdistetty";
        private SolidColorBrush STATE_COLOR_GREEN = Brushes.Green;
        private SolidColorBrush STATE_COLOR_RED = Brushes.Red;

        // Tank parameters
        private const int TANK_MAX_VALUE = 300;
        private const int TANK_MIN_VALUE = 0;

        // Sequence UI parameters
        private const string PARAMETERS_NOT_CONFIRMED = "Lukitsemattomia muutoksia!";
        private const string PARAMETERS_INCORRECT = "Parametrit virheellisiä.";
        private const string PARAMETERS_CONFIRMED = "Muutokset tallennettu.";


        #endregion

        #region CONFIGURABLE PARAMETERS

        private const int THREAD_DELAY_MS = 10;
        private const string CLIENT_URL = "opc.tcp://127.0.0.1:8087";

        #endregion

        #region CLASS VARIABLES

        private int State;

        #endregion

        #region OPC

        private Tuni.MppOpcUaClientLib.ConnectionParamsHolder mConnectionParamsHolder;
        private Tuni.MppOpcUaClientLib.MppClient mMppClient;

        #endregion

        #region OBJECTS

        Controller CTR = new Controller();  // Object for calling the Controller()

        #endregion


        public MainWindow()
        {
            // Set internal variables
            State = STATE_DISCONNECTED;
            mConnectionParamsHolder = new Tuni.MppOpcUaClientLib.ConnectionParamsHolder(CLIENT_URL);

            InitializeComponent();

            InitUI(); // Init static UI elemets

            CreateProcessConnection(); // Try to establish process connection

            UpdateValues(); // Update UI values

            UpdateControl(); // Update system controls 

            ResetUIParameters();    // Loads the default parameter values

            new Thread(() => // Start control thread
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
                    //Console.WriteLine("{0} ControlThread tick", DateTime.Now.ToString("hh:mm:ss"));

                    // TODO check connection status
                    
                    // Get values

                    // Update controls

                    // Update values

                    switch (State)
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
                            Console.WriteLine("ERROR: UpdateValues() switch default statement called");
                            break;
                    }

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
            textBox_impregnation_time.IsEnabled = isEnabled;
        }

        private void UpdateValues()
        {
            switch (State)
            {
                case STATE_FAILSAFE:
                    label_connection_status.Content = STATE_DISCONNECTED_STRING; // TODO Dynamic value needed
                    label_connection_status.Foreground = STATE_COLOR_RED;
                    label_control_status.Content = STATE_FAILSAFE_STRING;
                    label_control_status.Foreground = STATE_COLOR_RED;
                    break;
                case STATE_DISCONNECTED:
                    label_connection_status.Content = STATE_DISCONNECTED_STRING;
                    label_connection_status.Foreground = STATE_COLOR_RED;
                    label_control_status.Content = STATE_DISCONNECTED_STRING;
                    label_control_status.Foreground = STATE_COLOR_RED;
                    break;
                case STATE_IDLE:
                    label_connection_status.Content = STATE_CONNECTED_STRING;
                    label_connection_status.Foreground = STATE_COLOR_GREEN;
                    label_control_status.Content = STATE_IDLE_STRING;
                    label_control_status.Foreground = STATE_COLOR_GREEN;
                    break;
                case STATE_RUNNING:
                    label_connection_status.Content = STATE_CONNECTED_STRING;
                    label_connection_status.Foreground = STATE_COLOR_GREEN;
                    label_control_status.Content = STATE_RUNNING_STRING;
                    label_control_status.Foreground = STATE_COLOR_GREEN;
                    break;
                default:
                    Console.WriteLine("ERROR: UpdateValues() switch default statement called");
                    break;
            }
        }

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

            UpdateValues();

            UpdateControl();
        }

        private void button_interrupt_process_Click(object sender, RoutedEventArgs e)
        {
            // TODO

            State = STATE_FAILSAFE;

            UpdateValues();

            UpdateControl();
        }

        private void button_connect_Click(object sender, RoutedEventArgs e)
        {
            CreateProcessConnection();

            UpdateValues();

            UpdateControl();
        }

        private void CreateProcessConnection()
        {
            try
            {
                mMppClient = new Tuni.MppOpcUaClientLib.MppClient(mConnectionParamsHolder);
                mMppClient.Init();

                State = STATE_IDLE;
            }
            catch (Exception ex)
            {
                // TODO reset params
                
                Console.WriteLine(ex.Message);
                State = STATE_DISCONNECTED;
            }
        }

        private void button_set_parameters_Click(object sender, RoutedEventArgs e)
        {
            SendParametersToController();
            
        }

        private void SendParametersToController() 
        {
            try
            {
                CTR.Cooking_time = Int32.Parse(textBox_cooking_time.Text);
                CTR.Cooking_pressure = Int32.Parse(textBox_cooking_pressure.Text);
                CTR.Cooking_temperature = Int32.Parse(textBox_cooking_temperature.Text);
                CTR.Impregnation_time = Int32.Parse(textBox_impregnation_time.Text);

                UpdateParameterUIStatus(1);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);

                UpdateParameterUIStatus(0);
            }
        }

        private void UpdateParameterUIStatus(int success) 
        {
            // success = 0: Not correct user input parameters.
            // success = 1: Sending the values succeeded.
            // success = -1: Reseting to a not-confirmed state.

            if (success == 1) {
                textblock_parameter_status.Text = PARAMETERS_CONFIRMED;
                textblock_parameter_status.Foreground = STATE_COLOR_GREEN;
            } 
            else if (success == 0)
            {
                textblock_parameter_status.Text = PARAMETERS_INCORRECT;
                textblock_parameter_status.Foreground = STATE_COLOR_RED;
            }
            else if (success == -1) {
                textblock_parameter_status.Text = PARAMETERS_NOT_CONFIRMED;
                textblock_parameter_status.Foreground = STATE_COLOR_RED;
            }
        }

        private void button_reset_parameters_Click(object sender, RoutedEventArgs e)
        {
            ResetUIParameters();

        }

        private void ResetUIParameters() 
        {
            textBox_cooking_pressure.Text = CTR.default_Cooking_pressure.ToString();
            textBox_cooking_time.Text = CTR.default_Cooking_time.ToString();
            textBox_cooking_temperature.Text = CTR.default_Cooking_temperature.ToString();
            textBox_impregnation_time.Text = CTR.default_Impregnation_time.ToString();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateParameterUIStatus(-1);
        }

    }
}
