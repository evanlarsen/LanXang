using System;
using System.Linq;
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

        public DbSet<FileUploadEntity> Files { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseTables());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<StoreHoursEntity>()
                .HasKey(e => e.ID)
                .ToTable("StoreHours", "LanXang");
            modelBuilder.Entity<StoreHoursEntity>()
                .Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<FileUploadEntity>()
                .HasKey(e => e.ID)
                .ToTable("FileUpload", "LanXang");

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
                    DropTable(context, "LanXang.FileUpload");
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

                AddDinnerMenuItems(context);
                AddSushiMenuItems(context);
                AddLunchMenuItems(context);
            }

            private void AddDinnerMenuItems(Repository context)
            {
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 0,
                    CategoryType = "Dinner",
                    Name = "Appetizers",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Spring Rolls", Description = "Deep fried spring rolls stuffed with vegetables", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawn Rolls", Description = "Deep fried marinated prawns warpped in wonton", Price = "$7.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Edamame", Description = "Japanese soybean, lightly boiled and sauteed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Krab* Rangoon", Description = "Wonton stuffed with krab* salad, cream cheese, and spinach", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Gyoza", Description = "Japanese-style vegtable dumpling (pork or shrimp for additional $1.00)", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Calamari", Description = "Deep fried lightly breaded calamari served with Thai sauce", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Tataki", Description = "Choose seared tuna or steak with ponzu sauce", Price = "$9.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 1,
                    CategoryType = "Dinner",
                    Name = "Salad",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Crying Tiger", Description = "Grilled steak mixed with lime juice, chili, onion, tomatoes, basil and cabbage leaves", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "House Salad", Description = "Fresh lettuce &amp; carrots with ginger or creamy garlic dressing", Price = "$2.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Seaweed Salad", Description = "Variety of seasoned seaweed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Cucumber Salad", Description = "Thai style seasoned cucumber mixed with krab*", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Seafood Salad", Description = "Variety of Sashimi fish, krab*, &amp; cucumber served with mild or spicy ponzu sauce", Price = "$8.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Octupus or Conch Salad", Description = "Choose octopus or conch, mixed with cucumber served with mild or spicy ponzu sauce", Price = "$8.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 2,
                    CategoryType = "Dinner",
                    Name = "Specialties",
                    Description = "Served with steamed Jasmine Rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, or Tofu", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Favorite Cashew", Description = "Stir-fried with brown sauce, vegetables and cashew nuts", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Thai Basil", Description = "Stir-fried Thai basil, chili, and vegetables", Price = "" },
                        new MenuItemEntity() { Sequence = 4, Name = "Sweet and Sour Sauce", Description = "Lighly breaded meat sauteed with vegetables and sweet &amp; sour sauce", Price = "" },
                        new MenuItemEntity() { Sequence = 5, Name = "Garden Delight", Description = "Stir-fried mixed vegetables, Shitake mushrooms, and Oyster sauce", Price = "" },
                        new MenuItemEntity() { Sequence = 6, Name = "Crispy Garlic", Description = "Stir-fried mixed vegetables with white pepper in a bed of broccoli and carrots, topped with crispy garlic and cilantro", Price = "" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 4,
                    CategoryType = "Dinner",
                    Name = "Signature Plates",
                    Description = "Comes with steamed rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Mom's Favorite (Kra Chai)", Description = "Combonation of prawn and chicken breasts stir-fried with slices of fresh Kra Chai root (Thai herb), curry paste and vegetables", Price = "$14.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Seafood Platters", Description = "Stir-fried combonation of seafood, chili paste and vegetables", Price = "$14.95" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 5,
                    CategoryType = "Dinner",
                    Name = "Totally Veggies",
                    Description = "(No Chicken Stock, No Egg, No Fish Sauce, No Oyster Sauce)",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Fried Rice Jay", Description = "Stir-fried rice with broccoli, onions, and tomatoes", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Pad Thai Jay", Description = "Stir-fried thin rice noodles with diced tofu, sprouts, and grounded peanuts", Price = "$10.25" },
                        new MenuItemEntity() { Sequence = 2, Name = "Pad See Ew Jay", Description = "Stir-fried big rice noodles with fried tofu and broccoli", Price = "$10.25" },
                        new MenuItemEntity() { Sequence = 3, Name = "Thai fried rice", Description = "Onion, broccoli, carrot, and egg with chicken or tofu or steak", Price = "$10.95" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 6,
                    CategoryType = "Dinner",
                    Name = "Curry Dishes",
                    Description = "Comes with steamed rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, Tofu or Vegges", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Red Curry", Description = "Red curry paste cooked with coconut milk, bamboo shoots, and basil", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Green Curry", Description = "Green curry paste cooled with coconut milk, eggplant, and Thai basil", Price = "" },
                        new MenuItemEntity() { Sequence = 4, Name = "Yellow Curry", Description = "Yellow curry paste cooked with coconut milk, carrots and potatoes", Price = "" },
                        new MenuItemEntity() { Sequence = 5, Name = "Panang Curry", Description = "Panang curr paste cooked wiht coconut milk, peppers, and lime leaves", Price = "" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 7,
                    CategoryType = "Dinner",
                    Name = "Noodles",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, Tofu or Vegges", Description = "", Price = "$7.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$8.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Pad Thai", Description = "Stir-fried rice noodles, eggs, bean sprouts and ground peanuts", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Pad Kee Mao", Description = "Stir-fried wide rice noodles with eggs, onion, chili paste, Thai basil, broccoli, and tomatoes", Price = "" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 8,
                    CategoryType = "Dinner",
                    Name = "Fried Rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Beef, Tofu or Vegges", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 9,
                    CategoryType = "Dinner",
                    Name = "Specialty Fried Rice",
                    Description = "Your choice of Chicken, Beef, Tofu, or Vegetables. Prawns add additional $1.00",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Pineapple Fried Rice", Description = "Stir-fry rice with pineapples, eggs, onion, broccoli, tomato and green onion", Price = "$9.95 (Prawns - $10.95)" },
                        new MenuItemEntity() { Sequence = 1, Name = "Basil Fried Rice", Description = "Stir-fry rice with eggs, onion, broccoli, tomato, green onion, red bell pepper and Thai basil", Price = "$9.95 (Prawns - $10.95)" },
                        new MenuItemEntity() { Sequence = 2, Name = "Special Fried Rice", Description = "Stir-fry rice with carrots, onion and broccoli", Price = "$9.95 (Prawns - $10.95)" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 10,
                    CategoryType = "Dinner",
                    Name = "Soup",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Vietnamese Pho Noodle Soup", Description = "Pho is a classic, quick meal from Southeast Asia. Its Served in a soup bowl with fresh herbs, fresh rice noodles, topped with chopped scallion, cilantro, fried garlic and your choice of Beef, Chicken, or Seafood", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 1, Name = "Tom Yum", Description = "Hot and sour soup with mushrooms with herbs &amp; lime juice", Price = "Large - $9.95/Small - $4.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "TomKa", Description = "Hot and sour coconut milk soup, mushrooms, with herbs &amp; lime juice", Price = "Large - $9.95/Small - $4.95" },
                        new MenuItemEntity() { Sequence = 3, Name = "Miso Soup", Description = "Tofu, seaweed, and soy bean paste", Price = "$2.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 11,
                    CategoryType = "Dinner",
                    Name = "Entrees",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Sizzling Teriyaki", Description = "Your choice of meat served in sizzling hot over tender stir-fried vegetables, smothered in teriyaki sauce and served with steamed rice", Price = "Chicken or steak - $12.00 / Salmon - $13.00 / Shrimp - $14.00 / Surf and Turf (Steak and Shrimp) - $16.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Tempura", Description = "Lightly fried in Japanese batter with your choice of the following", Price = "Vegetables - $10.00 / Chicken and Vegetables - $11.00 / Shrimp and Vegetables - $12.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Combination", Description = "Tempura (shrimp and vegetable) and Teriyaki (Chicken and Steak)", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Tilapia", Description = "Deep fried tilapia fillet served with vegetable and sweet spicy Thai chili sauce", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Yakisoba", Description = "Pan fried yellow noodles with chicken and vegetables", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Tempura Udon", Description = "Wide Japanese noodles in broth with shrimp and vegetables tempura served on the side", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Unagi Donburi", Description = "BBQ Eel served over a bed of rice", Price = "$12.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Side of Steamed Rice", Description = "", Price = "$2.00" },
                        new MenuItemEntity() { Sequence = 8, Name = "Side of House Vegetable Fried Rice", Description = "", Price = "$4.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 12,
                    CategoryType = "Dinner",
                    Name = "Hibachi Specials",
                    Description = "All Hibachi dinners inclue Hibahi vegetable, Hibachi rice, and your choice of house soup or house salad with your choice of dressing (ginger, creamy garlic or ranch)",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Hibachi Vegetables", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 1, Name = "Hibachi Chicken", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 2, Name = "Hibachi Steak", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 3, Name = "Hibachi Shrimp", Description = "", Price = "$7.99" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 13,
                    CategoryType = "Dinner",
                    Name = "Sashimi Combinations",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "12 slices of raw", Price = "$18.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "15 slices of raw", Price = "$22.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "27 slices of raw", Price = "$42.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 14,
                    CategoryType = "Dinner",
                    Name = "Sushi Combinations",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "7 pieces of nigiri sushi and a California Roll", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "9 pieces of nigiri sushi nd a shrimp tempura", Price = "$19.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "16 pieces of nigiri sushi, California roll, shrimp tempura, and Philedelphia roll", Price = "$35.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "", Description = "Veggie Combination - veggie roll and seven pieces of vegetable nigri sushi", Price = "$13.00" }
                    }
                });

                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 15,
                    CategoryType = "Dinner",
                    Name = "Sushi Love Boats",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "7 Pieces of nigiri sushi, 8 pieces of sashimi, California roll and seafood salad", Price = "$35.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "9 pieces of nigiri sushi, 12 pieces of sashimi sushi, California roll, shrimp tempura roll, white orchid roll and seafood", Price = "$45.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "12 pieces of nigiri sushi, 15 pieces of sashimi, California roll, shrimp tempura roll, white orchid roll, and seafood salad", Price = "$35.00" }
                    }
                });
            }

            private void AddSushiMenuItems(Repository context)
            {
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 0,
                    CategoryType = "Sushi",
                    Name = "Special Rolls",
                    Description = "Made to your order by the sushi chef",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "CIRCA SUSHI Roll", Description = "eel, c.c., avacaho, smoked salmon wrapped in soy paper, topped w/ eel sauce", Price = "$12.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Cucumber Wrapped", Description = "spicy tuna, krab*, c.c. &amp; avocado wrapped in cucumber", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Dad's Favorite", Description = "eel, c.c. &amp; avocado inside; deep fried, topped with shrimp tempura, c.c. &amp; eel sauce", Price = "$11.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Eel Lover", Description = "eel, topped with shrimp tempura, cc. &amp; eel sauce", Price = "$12.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Firecracker", Description = "shrimp tempura, salmon, avocado, cucumber &amp; masago inside; wrapped with soy paper &amp; topped with crispy tomato", Price = "$13.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "FSU Roll", Description = "lobster tempura, masago &amp; cucumber inside; eel &amp; avacado outside", Price = "$14.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Hot Mama", Description = " eel, avocado, cucumber, c.c., red chilli sauce&amp; crispy tempura flake inside; wrapped in soy paper &amp; topped with spicy tuna", Price = "$13.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Hungry Man Roll", Description = "tuna, fresh salmon, white fish, spicy tuna, krab* salad, shrimp tempura, krab*, avocado &amp; masago &amp; sesame outside", Price = "$14.00" },
                        new MenuItemEntity() { Sequence = 8, Name = "Miami Heat", Description = "tuna, salmon, krab*, cucumber &amp; red shilli saucxe inside; masago &amp; sesame outside", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 9, Name = "Most Wanted", Description = "tuna, salmon, krab*, cucumber &amp; red shilli sauce inside; masago &amp; sesame outside", Price = "$13.00" },
                        new MenuItemEntity() { Sequence = 10, Name = "New Orleans", Description = "spicy crawish &amp; asparagus on inside, half krab* salad &amp; half spicy tuna on outside", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 11, Name = "Ocean Wave", Description = "Crispy krab* &amp; cucumber inside; steamed shrimp &amp; avocado outside", Price = "$11.00" },
                        new MenuItemEntity() { Sequence = 12, Name = "Seafood Roll", Description = "tuna, almon, &amp; white fish, wrapped in cucumber &amp; served w/ ponzu sauce", Price = "$11.00" },
                        new MenuItemEntity() { Sequence = 13, Name = "Shogun", Description = "tuna, salmon, white fish, eel, c.c., cucumber &amp; avocado; in spicy soy paper", Price = "$13.00" },
                        new MenuItemEntity() { Sequence = 14, Name = "Soft-Shell Crab", Description = "soft-shell crab, krab* salad, avocado &amp; masago w/ eel sauce", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 15, Name = "Super Crunch", Description = "eel, c.c., spicy tuna &amp; krab* salad; deep fried &amp; served w/ eel sauce &amp; spicy mayo", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 16, Name = "Volcano", Description = "California roll topped with spicy krab*", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 17, Name = "White Orchid", Description = "krab* salad, c.c. &amp; shrimp tempura wrapped in shite soy bean paper", Price = "$10.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 1,
                    CategoryType = "Sushi",
                    Name = "Makimono Sushi",
                    Description = "Fresh Makimono rolls made to your order (1 roll)",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "California", Description = "krab*, avocado, cucumber and masago", Price = "$5.50" },
                        new MenuItemEntity() { Sequence = 1, Name = "Philadelphia", Description = "smoked salmon, chream cheese and avocado", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Shrimp Tempura Roll", Description = "shrimp tempura and eel sauce", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "BBQ Eel", Description = "eel and avocado", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Spicy Tuna", Description = "chopped spicy tuna and cucumber", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Veggie", Description = "avocado, asparagus, cucumber", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Salmon Skin", Description = "roasted salmon skin, cucumbe and eel sauce", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Tekka", Description = "tuna", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 8, Name = "Kappa", Description = "cucumber", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 9, Name = "Krab Salad", Description = "krab* salad and asparagus", Price = "$6.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 2,
                    CategoryType = "Sushi",
                    Name = "Nigiri Sushi",
                    Description = "Fresh Nigiri sushi made to your order",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Maguro", Description = "tuna", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Hamachi", Description = "yellow tail", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Sake", Description = "fresh salmon", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Sake Smoked", Description = "smoked salmon", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Uni", Description = "sea urchin", Price = "$7.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Masago", Description = "flying fish roe", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Ikura", Description = "salmon roe", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Tako", Description = "octopus", Price = "$4.50" },
                        new MenuItemEntity() { Sequence = 8, Name = "Amaebi", Description = "sweet shrimp", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 9, Name = "Ika", Description = "squid", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 10, Name = "Kaibashira", Description = "scallop", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 11, Name = "Saba", Description = "mackerel", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 12, Name = "Horagai", Description = "conch", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 13, Name = "Kani", Description = "imitation crab", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 14, Name = "Ebi", Description = "shrimp", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 15, Name = "Unagi", Description = "eel", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 16, Name = "Spicy Tobiko", Description = "spicy fish eggs", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 17, Name = "Uzara", Description = "quail eggs", Price = "$3.00" },
                        new MenuItemEntity() { Sequence = 18, Name = "Blue Fin Tuna", Description = "", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 19, Name = "White Tuna", Description = "", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 20, Name = "Tilapia", Description = "", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 21, Name = "Tamago", Description = "", Price = "$3.00" },
                    }
                });
            }

            private void AddLunchMenuItems(Repository context)
            {
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 0,
                    CategoryType = "Lunch",
                    Name = "Appetizers",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Spring Rolls", Description = "Deep fried spring rolls stuffed with vegetables", Price = "$4.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawn Rolls", Description = "Deep fried marinated prawns warpped in wonton", Price = "$7.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Edamame", Description = "Japanese soybean, lightly boiled and sauteed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Krab* Rangoon", Description = "Wonton stuffed with krab* salad, cream cheese, and spinach", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Gyoza", Description = "Japanese-style vegtable dumpling (pork or shrimp for additional $1.00)", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Calamari", Description = "Deep fried lightly breaded calamari served with Thai sauce", Price = "$6.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Tataki", Description = "Choose seared tuna or steak with ponzu sauce", Price = "$9.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 1,
                    CategoryType = "Lunch",
                    Name = "Salad",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Crying Tiger", Description = "Grilled steak mixed with lime juice, chili, onion, tomatoes, basil and cabbage leaves", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "House Salad", Description = "Fresh lettuce &amp; carrots with ginger or creamy garlic dressing", Price = "$2.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Seaweed Salad", Description = "Variety of seasoned seaweed", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Cucumber Salad", Description = "Thai style seasoned cucumber mixed with krab*", Price = "$5.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Seafood Salad", Description = "Variety of Sashimi fish, krab*, &amp; cucumber served with mild or spicy ponzu sauce", Price = "$8.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Octupus or Conch Salad", Description = "Choose octopus or conch, mixed with cucumber served with mild or spicy ponzu sauce", Price = "$8.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 2,
                    CategoryType = "Lunch",
                    Name = "Specialties",
                    Description = "Served with steamed Jasmine Rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, or Tofu", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Favorite Cashew", Description = "Stir-fried with brown sauce, vegetables and cashew nuts", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Thai Basil", Description = "Stir-fried Thai basil, chili, and vegetables", Price = "" },
                        new MenuItemEntity() { Sequence = 4, Name = "Sweet and Sour Sauce", Description = "Lighly breaded meat sauteed with vegetables and sweet &amp; sour sauce", Price = "" },
                        new MenuItemEntity() { Sequence = 5, Name = "Garden Delight", Description = "Stir-fried mixed vegetables, Shitake mushrooms, and Oyster sauce", Price = "" },
                        new MenuItemEntity() { Sequence = 6, Name = "Crispy Garlic", Description = "Stir-fried mixed vegetables with white pepper in a bed of broccoli and carrots, topped with crispy garlic and cilantro", Price = "" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 3,
                    CategoryType = "Lunch",
                    Name = "Signature Plates",
                    Description = "Comes with steamed rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Mom's Favorite (Kra Chai)", Description = "Combonation of prawn and chicken breasts stir-fried with slices of fresh Kra Chai root (Thai herb), curry paste and vegetables", Price = "$14.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Seafood Platters", Description = "Stir-fried combonation of seafood, chili paste and vegetables", Price = "$14.95" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 4,
                    CategoryType = "Lunch",
                    Name = "Totally Veggies",
                    Description = "(No Chicken Stock, No Egg, No Fish Sauce, No Oyster Sauce)",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Fried Rice Jay", Description = "Stir-fried rice with broccoli, onions, and tomatoes", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Pad Thai Jay", Description = "Stir-fried thin rice noodles with diced tofu, sprouts, and grounded peanuts", Price = "$10.25" },
                        new MenuItemEntity() { Sequence = 2, Name = "Pad See Ew Jay", Description = "Stir-fried big rice noodles with fried tofu and broccoli", Price = "$10.25" },
                        new MenuItemEntity() { Sequence = 3, Name = "Thai fried rice - $9.95", Description = "Onion, broccoli, carrot, and egg with chicken or tofu or steak", Price = "$10.95" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 5,
                    CategoryType = "Lunch",
                    Name = "Curry Dishes",
                    Description = "Comes with steamed rice",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, Tofu or Vegges ", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Red Curry", Description = "Red curry paste cooked with coconut milk, bamboo shoots, and basil", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Green Curry", Description = "Green curry paste cooled with coconut milk, eggplant, and Thai basil", Price = "" },
                        new MenuItemEntity() { Sequence = 4, Name = "Yellow Curry", Description = "Yellow curry paste cooked with coconut milk, carrots and potatoes", Price = "" },
                        new MenuItemEntity() { Sequence = 5, Name = "Panang Curry", Description = "Panang curr paste cooked wiht coconut milk, peppers, and lime leaves", Price = "" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 6,
                    CategoryType = "Lunch",
                    Name = "Noodles",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Steak, Tofu or Vegges ", Description = "", Price = "$7.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$8.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Pad Thai", Description = "Stir-fried rice noodles, eggs, bean sprouts and ground peanuts", Price = "" },
                        new MenuItemEntity() { Sequence = 3, Name = "Pad Kee Mao", Description = "Stir-fried wide rice noodles with eggs, onion, chili paste, Thai basil, broccoli, and tomatoes", Price = "" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 7,
                    CategoryType = "Lunch",
                    Name = "Fried Rice",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Chicken, Beef, Tofu or Vegges ", Description = "", Price = "$9.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Prawns", Description = "", Price = "$10.95" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 8,
                    CategoryType = "Lunch",
                    Name = "Specialty Fried Rice",
                    Description = "You choice of Chicken, Beef, Tofu, or Vegetables. Prawns add additional $1.00",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Pineapple Fried Rice ", Description = "Stir-fry rice with pineapples, eggs, onion, broccoli, tomato and green onion ", Price = "$9.95 / Prawns - $10.95" },
                        new MenuItemEntity() { Sequence = 1, Name = "Basil Fried Rice", Description = "Stir-fry rice with eggs, onion, broccoli, tomato, green onion, red bell pepper and Thai basil", Price = "$9.95 / Prawns - $10.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "Special Fried Rice", Description = "Stir-fry rice with carrots, onion and broccoli", Price = "$9.95 / Prawns - $10.95" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 9,
                    CategoryType = "Lunch",
                    Name = "Soup",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Vietnamese Pho Noodle Soup", Description = "Pho is a classic, quick meal from Southeast Asia. Its Served in a soup bowl with fresh herbs, fresh rice noodles, topped with chopped scallion, cilantro, fried garlic and your choice of Beef, Chicken, or Seafood", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 1, Name = "Tom Yum", Description = "Hot and sour soup with mushrooms with herbs &amp; lime juice", Price = "Large - $9.95 / Small - $4.95" },
                        new MenuItemEntity() { Sequence = 2, Name = "TomKa", Description = "Hot and sour coconut milk soup, mushrooms, with herbs &amp; lime juice", Price = "Large - $9.95 / Small - $4.95" },
                        new MenuItemEntity() { Sequence = 3, Name = "Miso Soup", Description = "Tofu, seaweed, and soy bean paste", Price = "$2.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 10,
                    CategoryType = "Lunch",
                    Name = "Entrees",
                    Description = "",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Sizzling Teriyaki", Description = "Your choice of meat served in sizzling hot over tender stir-fried vegetables, smothered in teriyaki sauce and served with steamed rice", Price = "Chicken or steak - $12.00 / Salmon - $13.00 / Shrimp - $14.00 / Surf and Turf (Steak and Shrimp) - $16.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "Tempura", Description = "Lightly fried in Japanese batter with your choice of the following", Price = "Vegetables - $10.00 / Chicken and Vegetables - $11.00 / Shrimp and Vegetables - $12.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "Combonation", Description = "Tempura (shrimp and vegetable) and Teriyaki (Chicken and Steak)", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "Tilapia", Description = "Deep fried tilapia fillet served with vegetable and sweet spicy Thai chili sauce", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 4, Name = "Yakisoba", Description = "Pan fried yellow noodles with chicken and vegetables", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 5, Name = "Tempura Udon", Description = "Wide Japanese noodles in broth with shrimp and vegetables tempura served on the side", Price = "$10.00" },
                        new MenuItemEntity() { Sequence = 6, Name = "Unagi Donburi", Description = "BBQ Eel served over a bed of rice", Price = "$12.00" },
                        new MenuItemEntity() { Sequence = 7, Name = "Side of Steamed Rice", Description = "", Price = "$2.00" },
                        new MenuItemEntity() { Sequence = 8, Name = "Side of House Vegetable Fried Rice", Description = "", Price = "$4.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 11,
                    CategoryType = "Lunch",
                    Name = "Hibachi Specials",
                    Description = "All Hibachi dinners inclue Hibahi vegetable, Hibachi rice, and your choice of house soup or house salad with your choice of dressing (ginger, creamy garlic or ranch)",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "Hibachi Vegetables", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 1, Name = "Hibachi Chicken", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 2, Name = "Hibachi Steak", Description = "", Price = "$7.99" },
                        new MenuItemEntity() { Sequence = 3, Name = "Hibachi Shrimp", Description = "", Price = "$7.99" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 12,
                    CategoryType = "Lunch",
                    Name = "Sashimi Combinations",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "12 slices of raw", Price = "$18.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "15 slices of raw", Price = "$22.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "27 slices of raw", Price = "$42.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 13,
                    CategoryType = "Lunch",
                    Name = "Sushi Combinations",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "7 pieces of nigiri sushi and a California Roll", Price = "$16.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "9 pieces of nigiri sushi nd a shrimp tempura", Price = "$19.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "16 pieces of nigiri sushi, California roll, shrimp tempura, and Philedelphia roll", Price = "$35.00" },
                        new MenuItemEntity() { Sequence = 3, Name = "", Description = "Veggie Combination - veggie roll and seven pieces of vegetable nigri sushi", Price = "$13.00" },
                    }
                });
                context.MenuCategories.Add(new MenuCategoryEntity()
                {
                    Sequence = 14,
                    CategoryType = "Lunch",
                    Name = "Sushi Love Boats",
                    Description = "Chef's choice of fresh fish",
                    MenuItems = new List<MenuItemEntity>()
                    {
                        new MenuItemEntity() { Sequence = 0, Name = "", Description = "7 Pieces of nigiri sushi, 8 pieces of sashimi, California roll and seafood salad", Price = "$35.00" },
                        new MenuItemEntity() { Sequence = 1, Name = "", Description = "9 pieces of nigiri sushi, 12 pieces of sashimi sushi, California roll, shrimp tempura roll, white orchid roll and seafood", Price = "$45.00" },
                        new MenuItemEntity() { Sequence = 2, Name = "", Description = "12 pieces of nigiri sushi, 15 pieces of sashimi, California roll, shrimp tempura roll, white orchid roll, and seafood salad", Price = "$35.00" },
                    }
                });
            }

            #endregion
        }
    }
}
