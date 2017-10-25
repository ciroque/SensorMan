using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.I2c;
using Windows.Devices.Enumeration;
using System.Diagnostics;

namespace SensorMan.I2C
{
    class I2CAccess
    {
        private IList<I2cDevice> m_Devices = new List<I2cDevice>();

        public async void Initialize()
        {
            //            I2cConnectionSettings settings = new I2cConnectionSettings(0);
            //            I2cDevice device = I2cController.GetControllersAsync

            string aqs = I2cDevice.GetDeviceSelector("I2C1");
            var dis = await DeviceInformation.FindAllAsync(aqs);

            if(dis.Count > 0)
            {
                const int minimumAddress = 8;
                const int maximumAddress = 77;

                for(byte address = minimumAddress; address <= maximumAddress; address++)
                {
                    var settings = new I2cConnectionSettings(address);
                    settings.BusSpeed = I2cBusSpeed.FastMode;
                    settings.SharingMode = I2cSharingMode.Shared;

                    using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
                    {
                        if(device != null)
                        {
                            try
                            {
                                byte[] writeBuffer = new byte[1] { 0 };
                                device.Write(writeBuffer);
                                this.m_Devices.Add(device);
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }

            Debug.WriteLine("WTF");
        }
    }
}


