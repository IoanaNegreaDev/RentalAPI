﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentalAPI.Persistance;

namespace RentalAPI.Migrations
{
    [DbContext(typeof(RentalDbContext))]
    partial class RentalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RentalAPI.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DomainId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DomainId = 1,
                            Name = "Truck"
                        },
                        new
                        {
                            Id = 2,
                            DomainId = 1,
                            Name = "Minivan"
                        },
                        new
                        {
                            Id = 3,
                            DomainId = 1,
                            Name = "Sedan"
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("RentalAPI.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("ExchangeRate")
                        .HasColumnType("real");

                    b.Property<int>("PaymentCurrencyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("PaymentCurrencyId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("RentalAPI.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Default = true,
                            Name = "USD"
                        },
                        new
                        {
                            Id = 2,
                            Default = false,
                            Name = "EUR"
                        },
                        new
                        {
                            Id = 3,
                            Default = false,
                            Name = "CAD"
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Damage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("DamageCost")
                        .HasColumnType("real");

                    b.Property<string>("DamageDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RentableItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RentableItemId");

                    b.ToTable("Damages");
                });

            modelBuilder.Entity("RentalAPI.Models.Domain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Domains");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Vehicles"
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Fuel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PricePerUnit")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Fuels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Diesel",
                            PricePerUnit = 1.4f
                        },
                        new
                        {
                            Id = 2,
                            Name = "Gas",
                            PricePerUnit = 1.2f
                        },
                        new
                        {
                            Id = 3,
                            Name = "Electricity",
                            PricePerUnit = 0.3f
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("RentalAPI.Models.Rentable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("DomainId")
                        .HasColumnType("int");

                    b.Property<float>("PricePerDay")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DomainId");

                    b.ToTable("Rentables");
                });

            modelBuilder.Entity("RentalAPI.Models.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("BasePrice")
                        .HasColumnType("real");

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RentedItemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("RentedItemId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("RentalAPI.Models.RentalDamage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DamageId")
                        .HasColumnType("int");

                    b.Property<int>("RentalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DamageId");

                    b.HasIndex("RentalId");

                    b.ToTable("RentalDamages");
                });

            modelBuilder.Entity("RentalAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "Administrator",
                            UserName = "Administrator"
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Vehicle", b =>
                {
                    b.HasBaseType("RentalAPI.Models.Rentable");

                    b.Property<int>("FuelId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Producer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength(true);

                    b.Property<int>("TankCapacity")
                        .HasColumnType("int");

                    b.HasIndex("FuelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("RentalAPI.Models.VehicleRental", b =>
                {
                    b.HasBaseType("RentalAPI.Models.Rental");

                    b.Property<bool>("FullTank")
                        .HasColumnType("bit");

                    b.Property<float>("FullTankPrice")
                        .HasColumnType("real");

                    b.ToTable("VehicleRentals");
                });

            modelBuilder.Entity("RentalAPI.Models.Minivan", b =>
                {
                    b.HasBaseType("RentalAPI.Models.Vehicle");

                    b.Property<int>("PassangersSeatsCount")
                        .HasColumnType("int");

                    b.ToTable("Minivans");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            PricePerDay = 30f,
                            FuelId = 1,
                            Model = "Fly",
                            Producer = "Volvo",
                            RegistrationNumber = "TM24RNT",
                            TankCapacity = 30,
                            PassangersSeatsCount = 8
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 2,
                            PricePerDay = 32f,
                            FuelId = 1,
                            Model = "Thunder",
                            Producer = "Dacia",
                            RegistrationNumber = "TM25RNT",
                            TankCapacity = 32,
                            PassangersSeatsCount = 9
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 2,
                            PricePerDay = 20f,
                            FuelId = 2,
                            Model = "Speed",
                            Producer = "Dacia",
                            RegistrationNumber = "TM29RNT",
                            TankCapacity = 30,
                            PassangersSeatsCount = 6
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Sedan", b =>
                {
                    b.HasBaseType("RentalAPI.Models.Vehicle");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Sedans");

                    b.HasData(
                        new
                        {
                            Id = 7,
                            CategoryId = 3,
                            PricePerDay = 18f,
                            FuelId = 2,
                            Model = "Special",
                            Producer = "Peugeot",
                            RegistrationNumber = "TM98RNT",
                            TankCapacity = 20,
                            Color = "Red"
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 3,
                            PricePerDay = 15f,
                            FuelId = 2,
                            Model = "Logan",
                            Producer = "Dacia",
                            RegistrationNumber = "TM10RNT",
                            TankCapacity = 20,
                            Color = "White"
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 3,
                            PricePerDay = 16f,
                            FuelId = 1,
                            Model = "Megane",
                            Producer = "Renault",
                            RegistrationNumber = "TM15RNT",
                            TankCapacity = 22,
                            Color = "Black"
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Truck", b =>
                {
                    b.HasBaseType("RentalAPI.Models.Vehicle");

                    b.Property<int>("CargoCapacity")
                        .HasColumnType("int");

                    b.ToTable("Trucks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            PricePerDay = 27f,
                            FuelId = 1,
                            Model = "Cargo",
                            Producer = "Mercedes",
                            RegistrationNumber = "TM68RNT",
                            TankCapacity = 30,
                            CargoCapacity = 20
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            PricePerDay = 37f,
                            FuelId = 1,
                            Model = "Jumbo",
                            Producer = "Volvo",
                            RegistrationNumber = "TM69RNT",
                            TankCapacity = 30,
                            CargoCapacity = 50
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            PricePerDay = 31f,
                            FuelId = 1,
                            Model = "Dumbo",
                            Producer = "Volvo",
                            RegistrationNumber = "TM39RNT",
                            TankCapacity = 30,
                            CargoCapacity = 40
                        });
                });

            modelBuilder.Entity("RentalAPI.Models.Category", b =>
                {
                    b.HasOne("RentalAPI.Models.Domain", "Domain")
                        .WithMany("Categories")
                        .HasForeignKey("DomainId")
                        .HasConstraintName("FK_Categories_Domains")
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("RentalAPI.Models.Contract", b =>
                {
                    b.HasOne("RentalAPI.Models.Client", "Client")
                        .WithMany("Contracts")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK_Contracts_Clients")
                        .IsRequired();

                    b.HasOne("RentalAPI.Models.Currency", "Currency")
                        .WithMany("Contracts")
                        .HasForeignKey("PaymentCurrencyId")
                        .HasConstraintName("FK_Contracts_Currencies")
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("RentalAPI.Models.Damage", b =>
                {
                    b.HasOne("RentalAPI.Models.Rentable", "Rentable")
                        .WithMany("Damages")
                        .HasForeignKey("RentableItemId")
                        .HasConstraintName("FK_Damages_Rentables")
                        .IsRequired();

                    b.Navigation("Rentable");
                });

            modelBuilder.Entity("RentalAPI.Models.RefreshToken", b =>
                {
                    b.HasOne("RentalAPI.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_RefreshTokens_User")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RentalAPI.Models.Rentable", b =>
                {
                    b.HasOne("RentalAPI.Models.Category", "Category")
                        .WithMany("Rentables")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Rentables_Categories")
                        .IsRequired();

                    b.HasOne("RentalAPI.Models.Domain", null)
                        .WithMany("Rentables")
                        .HasForeignKey("DomainId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RentalAPI.Models.Rental", b =>
                {
                    b.HasOne("RentalAPI.Models.Contract", "Contract")
                        .WithMany("Rentals")
                        .HasForeignKey("ContractId")
                        .HasConstraintName("FK_Rentals_Contracts")
                        .IsRequired();

                    b.HasOne("RentalAPI.Models.Rentable", "RentedItem")
                        .WithMany("Rentals")
                        .HasForeignKey("RentedItemId")
                        .HasConstraintName("FK_Rentals_Rentables")
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("RentedItem");
                });

            modelBuilder.Entity("RentalAPI.Models.RentalDamage", b =>
                {
                    b.HasOne("RentalAPI.Models.Damage", "Damage")
                        .WithMany("RentalDamages")
                        .HasForeignKey("DamageId")
                        .HasConstraintName("FK_RentalDamages_Damage")
                        .IsRequired();

                    b.HasOne("RentalAPI.Models.Rental", "Rental")
                        .WithMany("RentalDamages")
                        .HasForeignKey("RentalId")
                        .HasConstraintName("FK_RentalDamages_Rentals")
                        .IsRequired();

                    b.Navigation("Damage");

                    b.Navigation("Rental");
                });

            modelBuilder.Entity("RentalAPI.Models.Vehicle", b =>
                {
                    b.HasOne("RentalAPI.Models.Fuel", "Fuel")
                        .WithMany("Vehicles")
                        .HasForeignKey("FuelId")
                        .HasConstraintName("FK_Vehicles_Fuels")
                        .IsRequired();

                    b.HasOne("RentalAPI.Models.Rentable", null)
                        .WithOne()
                        .HasForeignKey("RentalAPI.Models.Vehicle", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Fuel");
                });

            modelBuilder.Entity("RentalAPI.Models.VehicleRental", b =>
                {
                    b.HasOne("RentalAPI.Models.Rental", null)
                        .WithOne()
                        .HasForeignKey("RentalAPI.Models.VehicleRental", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RentalAPI.Models.Minivan", b =>
                {
                    b.HasOne("RentalAPI.Models.Vehicle", null)
                        .WithOne()
                        .HasForeignKey("RentalAPI.Models.Minivan", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RentalAPI.Models.Sedan", b =>
                {
                    b.HasOne("RentalAPI.Models.Vehicle", null)
                        .WithOne()
                        .HasForeignKey("RentalAPI.Models.Sedan", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RentalAPI.Models.Truck", b =>
                {
                    b.HasOne("RentalAPI.Models.Vehicle", null)
                        .WithOne()
                        .HasForeignKey("RentalAPI.Models.Truck", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RentalAPI.Models.Category", b =>
                {
                    b.Navigation("Rentables");
                });

            modelBuilder.Entity("RentalAPI.Models.Client", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("RentalAPI.Models.Contract", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("RentalAPI.Models.Currency", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("RentalAPI.Models.Damage", b =>
                {
                    b.Navigation("RentalDamages");
                });

            modelBuilder.Entity("RentalAPI.Models.Domain", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Rentables");
                });

            modelBuilder.Entity("RentalAPI.Models.Fuel", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("RentalAPI.Models.Rentable", b =>
                {
                    b.Navigation("Damages");

                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("RentalAPI.Models.Rental", b =>
                {
                    b.Navigation("RentalDamages");
                });

            modelBuilder.Entity("RentalAPI.Models.User", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
