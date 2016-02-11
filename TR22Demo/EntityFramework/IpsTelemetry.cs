namespace TR22Demo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IpsTelemetry")]
    public partial class IpsTelemetry
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ExternalIp { get; set; }

        [StringLength(50)]
        public string InternalIp { get; set; }

        [StringLength(50)]
        public string MacAddress { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        [StringLength(50)]
        public string DeviceGuid { get; set; }

        public DateTime? OriginationTimeStamp { get; set; }

        [StringLength(50)]
        public string CurrentAppVersion { get; set; }

        public string ConfigDoc { get; set; }

        public int? CurrentDeviceStatus { get; set; }

        [StringLength(50)]
        public string FriendlyLocationName { get; set; }

        [StringLength(50)]
        public string FriendlyDeviceName { get; set; }
    }
}
