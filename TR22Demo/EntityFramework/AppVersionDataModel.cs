namespace TR22Demo.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppVersionDataModel : DbContext
    {
        public AppVersionDataModel()
            : base("name=AppVersionDataModel")
        {
        }

        public virtual DbSet<AppVersionMetadata> AppVersionMetadatas { get; set; }
        public virtual DbSet<IpsTelemetry> IpsTelemetries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppVersionMetadata>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<AppVersionMetadata>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}
