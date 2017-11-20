using System;
using System.Diagnostics;

using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

namespace SensorMan.SPI
{
    enum Sensor : int
    {
        Temperature = 0,
        Volume = 1,
        Gaseous = 2,
        Touch = 3
    }

    class SPIAccess
    {
        private SpiDevice _mcp3008;
        private byte[] _sensorMap = new byte[] { 0x80, 0x90, 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0 };

        private async void GetTheThing()
        {
            //using SPI0 on the Pi
            var spiSettings = new SpiConnectionSettings(0);//for spi bus index 0
            spiSettings.ClockFrequency = 3600000; //3.6 MHz
            spiSettings.Mode = SpiMode.Mode0;

            string spiQuery = SpiDevice.GetDeviceSelector("SPI0");
            //using Windows.Devices.Enumeration;
            var deviceInfo = await DeviceInformation.FindAllAsync(spiQuery);
            if (deviceInfo != null && deviceInfo.Count > 0)
            {
                _mcp3008 = await SpiDevice.FromIdAsync(deviceInfo[0].Id, spiSettings);
            }
            else
            {
                Debug.WriteLine("SPI Device Not Found :-(");
            }
        }

        public void Initialize()
        {
            GetTheThing();
        }

        public int ReadSensor(Sensor sensor)
        {
            var transmitBuffer = new byte[3] { 1, _sensorMap[(int)sensor], 0x00 };
            var receiveBuffer = new byte[3] { 0, 0, 0 };

            try
            {
                Debug.WriteLine("Trying device: " + _mcp3008.DeviceId);
                _mcp3008.TransferFullDuplex(transmitBuffer, receiveBuffer);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var result = ((receiveBuffer[1] & 3) << 8) + receiveBuffer[2];

            return result;
        }
    }
}
