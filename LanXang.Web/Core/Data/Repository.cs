using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using LanXang.Web.Core.Entities;
using System.Collections.Generic;

namespace LanXang.Web.Core.Data
{
    public class Repository : DbContext
    {
        public Repository()
            : base("LanXang")
        {
        }

        public DbSet<StoreHoursEntity> StoreHours { get; set; }

        public DbSet<MenuCategoryEntity> MenuCategories { get; set; }

        public DbSet<MenuItemEntity> MenuItems { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseTables());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<StoreHoursEntity>()
                .HasKey(e => e.ID)
                .ToTable("StoreHours", "LanXang");
            modelBuilder.Entity<StoreHoursEntity>()
                .Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuCategoryEntity>()
                .HasKey(e => e.ID)
                .ToTable("MenuCategory", "LanXang");
            modelBuilder.Entity<MenuCategoryEntity>()
                .HasMany(e => e.MenuItems);
            modelBuilder.Entity<MenuCategoryEntity>()
                .Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MenuItemEntity>()
                .HasKey(e => e.ID)
                .ToTable("MenuItem", "LanXang");
            modelBuilder.Entity<MenuItemEntity>()
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
                    DropTable(context, "dbo.EdmMetadata");

                    // remove all tables
                    DropTable(context, "LanXang.StoreHours");
                    DropTable(context, "LanXang.MenuItem");
                    DropTable(context, "LanXang.MenuCategory");

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

            private void DropTable(Repository context, string tableName)
            {
                context.Database.ExecuteSqlCommand(String.Format("IF OBJECT_ID('{0}','U') IS NOT NULL \r\n DROP TABLE {0}", tableName));
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


                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 1,
                    CategoryType = "Dinner",
                    Name = "Appetizers",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 1, Name = "Spring Rolls", Description = "Deep fried spring rolls stuffed with vegetables", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Prawn Rolls", Description = "Deep fried marinated prawns warpped in wonton", Price = "$7.95" },
                        new MenuItemEntity() { Sequence = 3, Name = "Edamame", Description = "Japanese soybean, lightly boiled and sauteed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Krab* Rangoon", Description = "Wonton stuffed with krab* salad, cream cheese, and spinach", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Gyoza", Description = "Japanese-style vegtable dumpling (pork or shrimp for additional $1.00)", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Calamari", Description = "Deep fried lightly breaded calamari served with Thai sauce", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Tataki", Description = "Choose seared tuna or steak with ponzu sauce", Price = "$9.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 2,
                    CategoryType = "Dinner",
                    Name = "Salad",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 1, Name = "Crying Tiger", Description = "Grilled steak mixed with lime juice, chili, onion, tomatoes, basil and cabbage leaves", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "House Salad", Description = "Fresh lettuce & carrots with ginger or creamy garlic dressing", Price = "$2.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Seaweed Salad", Description = "Variety of seasoned seaweed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Cucumber Salad", Description = "Thai style seasoned cucumber mixed with krab*", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Seafood Salad", Description = "Variety of Sashimi fish, krab*, & cucumber served with mild or spicy ponzu sauce", Price = "$8.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Octupus or Conch Salad", Description = "Choose octopus or conch, mixed with cucumber served with mild or spicy ponzu sauce", Price = "$8.00" }
                    }
                });
            }

            #endregion
        }
    }
}
