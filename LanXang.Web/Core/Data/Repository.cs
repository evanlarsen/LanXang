using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LanXang.Web.Core.Entities;
using System.Data.Entity.Infrastructure;
using System;

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
            Database.SetInitializer(new DropCreateDatabaseTables());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<StoreHoursEntity>()
                .HasKey(sh => sh.ID)
                .ToTable("StoreHours", "LanXang");

            modelBuilder.Entity<StoreHoursEntity>()
                .Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }


        public class DropCreateDatabaseTables : IDatabaseInitializer<Repository>
        {
            #region IDatabaseInitializer<Context> Members

            public void InitializeDatabase(Repository context)
            {
                bool dbExists = context.Database.Exists();
                if (dbExists)
                {
                    context.Database.ExecuteSqlCommand("DROP TABLE dbo.EdmMetadata");

                    // remove all tables
                    context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\", @whereand = \"and uid = (SELECT schema_id FROM sys.schemas WHERE name = 'LanXang')\"");

                    // create all tables
                    var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    Seed(context);
                    context.SaveChanges();
                }
                else
                {
                    throw new ApplicationException("No database instance");
                }
            }

            #endregion

            #region Methods

            protected virtual void Seed(Repository context)
            {
                context.StoreHours.Add(new StoreHoursEntity()
                {
                    Line1 = "Mon-Thurs: 11am-10pm",
                    Line2 = "Friday: 11am-10:30pm",
                    Line3 = "Saturday: 12-10:30pm"
                });
            }

            #endregion
        }
    }
}
