using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

namespace SensorMan.SPI
{

    class SPIAccess
    {
        IList<SpiDevice> m_Devices = new List<SpiDevice>();

        public async void Initialize()
        {
            string deviceSelector = SpiDevice.GetDeviceSelector();
            var deviceInformations = await DeviceInformation.FindAllAsync(deviceSelector);
            
            if(deviceInformations.Count > 0)
            {
                const int minimumAddress = 0;
                const int maximumAddress = 1;

                for(byte address = minimumAddress; address <= maximumAddress; address++)
                {
                    Debug.WriteLine(">>> Checking Address: " + address);

                    var settings = new SpiConnectionSettings(address);

                    settings.SharingMode = SpiSharingMode.Shared;

                    using (SpiDevice device = await SpiDevice.FromIdAsync(deviceInformations[0].Id, settings))
                    {
                        if(device != null)
                        {
                            try
                            {
                                byte[] buffer = new byte[1] { 0 };
                                device.Write(buffer);


                                m_Devices.Add(device);
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }

            Debug.WriteLine("DONE");
        }
    }
}
