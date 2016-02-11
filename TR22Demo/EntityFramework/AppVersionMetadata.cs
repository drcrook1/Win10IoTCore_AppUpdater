namespace TR22Demo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppVersionMetadata")]
    public partial class AppVersionMetadata
    {
        public int Id { get; set; }

        public DateTime? PublishedTime { get; set; }

        [StringLength(100)]
        public string Version { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
