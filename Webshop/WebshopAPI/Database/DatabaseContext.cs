namespace WebshopAPI.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ZipCode> ZipCode { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasOne(b => b.Customer)
            .WithMany(a => a.Orders)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
            .HasOne(b => b.Address)
            .WithMany(a => a.Orders)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>()
            .HasOne(b => b.Address)
            .WithOne(a => a.Customer)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Login>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Sko"
                },
                new Category
                {
                    Id = 2,
                    Name = "Bukser"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Blå sko",
                    Description = "Den blå sko",
                    Price = 200,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Sort Jeans",
                    Description = "De sorte Wrangler",
                    Price = 199.99M,
                    CategoryId = 2
                });
            modelBuilder.Entity<ZipCode>().HasData(
                new ZipCode
                {
                    Id = 2750,
                    City = "Ballerup"
                },
                new ZipCode
                {
                    Id = 2300,
                    City = "København S"
                },
                new ZipCode
                {
                    Id = 2650,
                    City = "Hvidore"
                },
                new ZipCode
                {
                    Id = 2720,
                    City = "Vanløse"
                },
                new ZipCode
                {
                    Id = 2100,
                    City = "København Ø"
                },
                new ZipCode
                {
                    Id = 1301,
                    City = "København K"
                },
                new ZipCode
                {
                    Id = 2200,
                    City = "København N"
                },
                new ZipCode
                {
                    Id = 2400,
                    City = "København NV"
                },
                new ZipCode
                {
                    Id = 2770,
                    City = "Kastrup"
                },
                new ZipCode
                {
                    Id = 2670,
                    City = "Greve"
                }
                );

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    StreetName = "Telegrafvej 9",
                    ZipCodeId = 2750,
                    Country = "Denmark"
                },
                new Address
                {
                    Id = 2,
                    StreetName = "Tænkevej",
                    ZipCodeId = 2300,
                    Country = "Denmark"
                }
                );
            modelBuilder.Entity<Login>().HasData(
                new Login
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("Passw0rd"),
                    Email = "WebshopAPI@gmail.com",
                    Role = Role.Admin
                },
                new Login
                {
                    Id = 2,
                    UserName = "Drenzy",
                    Password = BCrypt.Net.BCrypt.HashPassword("Passw0rd"),
                    Email = "Drenzy@gmail.com",
                    Role = Role.User
                }
                );
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Daniel Nikolaj",
                    AddressId = 1,
                    PhoneNr = "12345678",
                    LoginId = 2
                });
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2002-01-12"),
                    StatusId = 4,
                    AddressId = 1
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2023-01-12"),
                    StatusId = 1,
                    AddressId = 2
                });
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    Price = 20,
                    Quantity = 1,
                    OrderId = 1,
                    ProductId = 1,
                }
                );
            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    Id = 1,
                    Name = "Received"
                },
                new Status
                {
                    Id = 2,
                    Name = "Paid"
                },
                new Status
                {
                    Id = 3,
                    Name = "Packing"
                },
                new Status
                {
                    Id = 4,
                    Name = "Sent"
                },
                new Status
                {
                    Id = 5,
                    Name = "Cancelled"
                },
                new Status
                {
                    Id = 6,
                    Name = "Remaining Order"
                });
            
        }
    }
}
