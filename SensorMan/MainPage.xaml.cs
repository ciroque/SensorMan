using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SensorMan.SPI;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SensorMan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer m_Timer = new DispatcherTimer();

        //private I2CAccess i2cAccess = new I2CAccess();
        private SPIAccess m_spiAccess = new SPIAccess();

        public MainPage()
        {
            this.InitializeComponent();

            this.m_Timer.Tick += HandleTimerTick;
            this.m_Timer.Interval = TimeSpan.FromMilliseconds(500);

            //i2cAccess.Initialize();
            m_spiAccess.Initialize();


            m_Timer.Start();
            Debug.WriteLine("MainPage::ctor");
        }

        private void HandleTimerTick(object sender, object e)
        {
            VolumeValue.Text = m_spiAccess.ReadSensor(Sensor.Volume).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void UpdateTemperatureButton_Click(object sender, RoutedEventArgs e)
        {
            int result = m_spiAccess.ReadSensor(Sensor.Temperature);

            var mv = result * (3300.0 / 1024.0);
            var tempC = mv / 10.0;
            var tempF = (tempC * 9.0 / 5.0) + 32;

            var calc = result * (3300.0 / 1024.0);
            var celsius = (calc / 10.0);
            var farenheit = (celsius * 9.0 / 5.0) + 32;
            TemperatureValue.Text = result.ToString();
        }

        private void UpdateVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            VolumeValue.Text = m_spiAccess.ReadSensor(Sensor.Volume).ToString();
        }

        private void UpdateGaseousButton_Click(object sender, RoutedEventArgs e)
        {
            GaseousValue.Text = m_spiAccess.ReadSensor(Sensor.Gaseous).ToString();
        }

        private void UpdateTouchButton_Click(object sender, RoutedEventArgs e)
        {
            TouchValue.Text = m_spiAccess.ReadSensor(Sensor.Touch).ToString();
        }
    }
}
