using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LanXang.Web.Core.Entities;

namespace LanXang.Web.Core.Data
{
    public class Repository : DbContext
    {
        public Repository()
            : base("LanXang")
        {
        }

        public DbSet<StoreHoursEntity> StoreHours { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<StoreHoursEntity>()
            //    .HasKey(sh => sh.ID)
            //    .ToTable("StoreHours", "LanXang");
        }
    }
}
