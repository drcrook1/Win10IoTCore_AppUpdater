using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace UpdateApp.AppData
{
    public sealed class AppDataTelemetry
    {
        private DeviceClient deviceClient;
        private string IoTHubConnString = "HostName=tr22Demo.azure-devices.net;DeviceId=device1;SharedAccessSignature=SharedAccessSignature sr=tr22Demo.azure-devices.net%2fdevices%2fdevice1&sig=Wf77csFuqDhmnWPRGU0%2fthxpOPV0aMRKWQg6fzXAjBA%3d&se=1457027297";
        private AppTelemetryData currentData;
        private string AppVersion;
        private string DeviceName;

        public AppDataTelemetry(string appVersion, string DeviceName)
        {
            this.AppVersion = appVersion;
            this.DeviceName = DeviceName;
            currentData = new AppTelemetryData();
            this.deviceClient = DeviceClient.CreateFromConnectionString(IoTHubConnString, TransportType.Http1);
        }

        public async void UpdateData()
        {
            try {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Pi");
                var task = await client.GetAsync("http://www.trackip.net/ip?json");
                var jsonString = await task.Content.ReadAsStringAsync();

                var rc = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));
                var rserializer = new DataContractJsonSerializer(typeof(LocationData));
                var ResultDeserialized = (LocationData)rserializer.ReadObject(rc);
                this.currentData.ExternalIp = ResultDeserialized.ip;
                this.currentData.Latitude = ResultDeserialized.latlong.Split(',')[0];
                this.currentData.Longitude = ResultDeserialized.latlong.Split(',')[1];
            }
            catch(Exception e)
            {

            }
            this.currentData.CurrentAppVersion = this.AppVersion;
            this.currentData.OriginationTimeStamp = DateTime.UtcNow;
            this.currentData.FriendlyDeviceName = this.DeviceName;
        }

        public async void BroadcastData()
        {
            string text = this.currentData.ToJSON();
            var msg = new Message(Encoding.UTF8.GetBytes(text));
            try
            {
                await deviceClient.SendEventAsync(msg);                            
            }
            catch(Exception e)
            {

            }
        }
    }
}
