using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateApp.AppData
{
    public sealed class AppTelemetryData
    {
            public int Id { get; set; }

            [StringLength(50)]
            public string ExternalIp { get; set; }

            [StringLength(50)]
            public string InternalIp { get; set; }

            [StringLength(50)]
            public string MacAddress { get; set; }

            public string Longitude { get; set; }

            public string Latitude { get; set; }

            [StringLength(50)]
            public string DeviceGuid { get; set; }

            public DateTimeOffset OriginationTimeStamp { get; set; }

            [StringLength(50)]
            public string CurrentAppVersion { get; set; }

            public string ConfigDoc { get; set; }

            public int? CurrentDeviceStatus { get; set; }

            [StringLength(50)]
            public string FriendlyLocationName { get; set; }

            [StringLength(50)]
            public string FriendlyDeviceName { get; set; }

            public string ToJSON()
            {
                return JsonConvert.SerializeObject(this);
            }
    }
}
