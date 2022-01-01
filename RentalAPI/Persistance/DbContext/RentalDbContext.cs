using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RentalAPI.Models;

#nullable disable

namespace RentalAPI.Persistance
{
    public partial class RentalDbContext : DbContext
    {
        public RentalDbContext()
        {
        }

        public RentalDbContext(DbContextOptions<RentalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Damage> Damages { get; set; }
        public virtual DbSet<EngineType> EngineTypes { get; set; }
        public virtual DbSet<FuelType> FuelTypes { get; set; }
        public virtual DbSet<Minivan> Minivans { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PricesPerFuelUnit> PricesPerFuelUnits { get; set; }
        public virtual DbSet<Rentable> Rentables { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<RentalDamage> RentalDamages { get; set; }
        public virtual DbSet<RentalStatus> RentalStatuses { get; set; }
        public virtual DbSet<Sedan> Sedans { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Truck> Trucks { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleContract> VehicleContracts { get; set; }
        public virtual DbSet<VehicleRental> VehicleRentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=RentalDbConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Domain>().HasData
            (
                new Domain { Id = 1, Name = "Vehicles" }
            );

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Mobile).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasOne(d => d.Client)
                     .WithMany(p => p.Contracts)
                     .HasForeignKey(d => d.ClientId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Contracts_Clients");
            });

            modelBuilder.Entity<Currency>().ToTable("Currencies");

            modelBuilder.Entity<Currency>().HasData
            (
                new Currency { Id = 1, Name = "USD", Default = true },
                new Currency { Id = 2, Name = "EUR", Default = false },
                new Currency { Id = 3, Name = "CAD", Default = false }
            );

            modelBuilder.Entity<Damage>(entity =>
            {
                entity.Property(e => e.DamageDescription).IsRequired();

                entity.HasOne(d => d.Rentable)
                    .WithMany(p => p.Damages)
                    .HasForeignKey(d => d.RentableItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Damages_Rentables");
            });

            modelBuilder.Entity<EngineType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Fuel)
                    .WithMany(p => p.EngineTypes)
                    .HasForeignKey(d => d.FuelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EngineTypes_FuelTypes");
            });


            modelBuilder.Entity<EngineType>().HasData
            (
                new EngineType { Id = 1, Name = "Diesel", FuelId = 1 },
                new EngineType { Id = 2, Name = "Gas", FuelId = 2 },
                new EngineType { Id = 3, Name = "Hybrid", FuelId = 2 }
            );

            modelBuilder.Entity<FuelType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<FuelType>().HasData
            (
                new FuelType { Id = 1, Name = "Diesel" },
                new FuelType { Id = 2, Name = "Gas" },
                new FuelType { Id = 3, Name = "Electricity" }
            );

            modelBuilder.Entity<Minivan>().ToTable("Minivans");

            modelBuilder.Entity<Minivan>().HasData
            (
                new Minivan { Id = 4,
                              CategoryId = 2,
                              PricePerDay = 30,
                              Producer = "Volvo",
                              Model = "Fly",
                              RegistrationNumber = "TM24RNT",
                              EngineTypeId = 1,
                              TankCapacity = 30,
                              PassangersSeatsCount = 8},
                new Minivan 
                {
                            Id = 5,
                            CategoryId = 2,
                            PricePerDay = 32,
                            Producer = "Dacia",
                            Model = "Thunder",
                            RegistrationNumber = "TM25RNT",
                            EngineTypeId = 1,
                            TankCapacity = 32,
                            PassangersSeatsCount = 9
                    },

                    new Minivan
                    {
                            Id = 6,
                            CategoryId = 2,
                            PricePerDay = 20,
                            Producer = "Dacia",
                            Model = "Speed",
                            RegistrationNumber = "TM29RNT",
                            EngineTypeId = 2,
                            TankCapacity = 30,
                            PassangersSeatsCount = 6
                    }
            );


            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_Contracts");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_Currencies");
            });

            modelBuilder.Entity<PricesPerFuelUnit>(entity =>
            {
                entity.ToTable("PricesPerFuelUnit");

                entity.HasOne(d => d.Fuel)
                    .WithMany(p => p.PricesPerFuelUnits)
                    .HasForeignKey(d => d.FuelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PricesPerFuelUnit_FuelTypes");
            });

            modelBuilder.Entity<PricesPerFuelUnit>().HasData
            (
                 new PricesPerFuelUnit { Id = 1, FuelId = 1, PricePerUnit = (float)3 },
                 new PricesPerFuelUnit { Id = 2, FuelId = 2, PricePerUnit = (float)2.9 },
                 new PricesPerFuelUnit { Id = 3, FuelId = 3, PricePerUnit = (float)0.5 }
            );


            modelBuilder.Entity<Rentable>(entity =>
            {                    
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rentables)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rentables_Categories");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
             //   entity.Property(prop => prop.BasePrice)
              //        .ValueGeneratedOnAddOrUpdate();

               // entity.Property(prop => prop.DamagePrice)
              //        .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.RentedItem)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.RentedItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rentals_Rentables");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rentals_Contracts");

                 entity.HasOne(d => d.Status)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rentals_Statuses");
            });

            modelBuilder.Entity<RentalDamage>(entity =>
            {
                entity.HasOne(d => d.Rental)
                    .WithMany(p => p.RentalDamages)
                    .HasForeignKey(d => d.RentalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentalDamages_Rentals");

                entity.HasOne(d => d.Damage)
                    .WithMany(p => p.RentalDamages)
                    .HasForeignKey(d => d.DamageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentalDamages_Damage");
            });

            modelBuilder.Entity<RentalStatus>().ToTable("RentalStatuses");
            modelBuilder.Entity<RentalStatus>().HasData
            (
                new RentalStatus { Id = 1, Name = "Reserved" },
                new RentalStatus { Id = 2, Name = "Active" },
                new RentalStatus { Id = 3, Name = "Closed" },
                new RentalStatus { Id = 4, Name = "Overdue" }
            );

            modelBuilder.Entity<Sedan>().ToTable("Sedans");

            modelBuilder.Entity<Sedan>().HasData
              (
                  new Sedan
                  {
                      Id = 7,
                      CategoryId = 3,
                      PricePerDay = 18,
                      Producer = "Peugeot",
                      Model = "Special",
                      RegistrationNumber = "TM98RNT",
                      EngineTypeId = 2,
                      TankCapacity = 20,
                      Color = "Red"
                  },
                  new Sedan
                  {
                      Id = 8,
                      CategoryId = 3,
                      PricePerDay = 15,
                      Producer = "Dacia",
                      Model = "Logan",
                      RegistrationNumber = "TM10RNT",
                      EngineTypeId = 2,
                      TankCapacity = 20,
                      Color = "White"
                  },

                new Sedan
                {
                    Id = 9,
                    CategoryId = 3,
                    PricePerDay = 16,
                    Producer = "Renault",
                    Model = "Megane",
                    RegistrationNumber = "TM15RNT",
                    EngineTypeId = 1,
                    TankCapacity = 22,
                    Color = "Black"
                }
              );

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Domains");
            });

            modelBuilder.Entity<Category>().HasData
            (
                new Category { Id = 1, DomainId = 1, Name = "Truck" },
                new Category { Id = 2, DomainId = 1, Name = "Minivan" },
                new Category { Id = 3, DomainId = 1, Name = "Sedan" }
            );

            modelBuilder.Entity<Truck>().ToTable("Trucks");

            modelBuilder.Entity<Truck>().HasData(
                new Truck
                {
                    Id = 1,
                    CategoryId = 1,
                    PricePerDay = 27,
                    Producer = "Mercedes",
                    Model = "Cargo",
                    RegistrationNumber = "TM68RNT",
                    EngineTypeId = 1,
                    TankCapacity = 30,
                    CargoCapacity = 20
                },
                new Truck
                {
                    Id = 2,
                    CategoryId = 1,
                    PricePerDay = 37,
                    Producer = "Volvo",
                    Model = "Jumbo",
                    RegistrationNumber = "TM69RNT",
                    EngineTypeId = 1,
                    TankCapacity = 30,
                    CargoCapacity = 50
                },
                new Truck
                {
                    Id = 3,
                    CategoryId = 1,
                    PricePerDay = 31,
                    Producer = "Volvo",
                    Model = "Dumbo",
                    RegistrationNumber = "TM39RNT",
                    EngineTypeId = 1,
                    TankCapacity = 30,
                    CargoCapacity = 40
                }
            );
   
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Producer)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.EngineType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.EngineTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicles_EngineTypes");        
            });





            modelBuilder.Entity<VehicleContract>().ToTable("VehicleContracts");

            modelBuilder.Entity<VehicleRental>().ToTable("VehicleRentals");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
