using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fuels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerUnit = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PaymentCurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Currencies",
                        column: x => x.PaymentCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Domains",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    PaidAmountInPaymentCurrency = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rentables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PricePerDay = table.Column<float>(type: "real", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentables_Categories",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rentables_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Damages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentableItemId = table.Column<int>(type: "int", nullable: false),
                    DamageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageCost = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Damages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Damages_Rentables",
                        column: x => x.RentableItemId,
                        principalTable: "Rentables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentedItemId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Contracts",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rentals_Rentables",
                        column: x => x.RentedItemId,
                        principalTable: "Rentables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Producer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    FuelId = table.Column<int>(type: "int", nullable: false),
                    TankCapacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Fuels",
                        column: x => x.FuelId,
                        principalTable: "Fuels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Rentables_Id",
                        column: x => x.Id,
                        principalTable: "Rentables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalDamages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    DamageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalDamages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalDamages_Damage",
                        column: x => x.DamageId,
                        principalTable: "Damages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalDamages_Rentals",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FullTank = table.Column<bool>(type: "bit", nullable: false),
                    FullTankPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRentals_Rentals_Id",
                        column: x => x.Id,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Minivans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PassangersSeatsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Minivans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Minivans_Vehicles_Id",
                        column: x => x.Id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sedans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sedans_Vehicles_Id",
                        column: x => x.Id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CargoCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trucks_Vehicles_Id",
                        column: x => x.Id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Default", "Name" },
                values: new object[,]
                {
                    { 1, true, "USD" },
                    { 2, false, "EUR" },
                    { 3, false, "CAD" }
                });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Vehicles" });

            migrationBuilder.InsertData(
                table: "Fuels",
                columns: new[] { "Id", "Name", "PricePerUnit" },
                values: new object[,]
                {
                    { 1, "Diesel", 1.4f },
                    { 2, "Gas", 1.2f },
                    { 3, "Electricity", 0.3f }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DomainId", "Name" },
                values: new object[] { 1, 1, "Truck" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DomainId", "Name" },
                values: new object[] { 2, 1, "Minivan" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DomainId", "Name" },
                values: new object[] { 3, 1, "Sedan" });

            migrationBuilder.InsertData(
                table: "Rentables",
                columns: new[] { "Id", "CategoryId", "DomainId", "PricePerDay" },
                values: new object[,]
                {
                    { 1, 1, null, 27f },
                    { 2, 1, null, 37f },
                    { 3, 1, null, 31f },
                    { 4, 2, null, 30f },
                    { 5, 2, null, 32f },
                    { 6, 2, null, 20f },
                    { 7, 3, null, 18f },
                    { 8, 3, null, 15f },
                    { 9, 3, null, 16f }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "FuelId", "Model", "Producer", "RegistrationNumber", "TankCapacity" },
                values: new object[,]
                {
                    { 1, 1, "Cargo", "Mercedes", "TM68RNT", 30 },
                    { 2, 1, "Jumbo", "Volvo", "TM69RNT", 30 },
                    { 3, 1, "Dumbo", "Volvo", "TM39RNT", 30 },
                    { 4, 1, "Fly", "Volvo", "TM24RNT", 30 },
                    { 5, 1, "Thunder", "Dacia", "TM25RNT", 32 },
                    { 6, 2, "Speed", "Dacia", "TM29RNT", 30 },
                    { 7, 2, "Special", "Peugeot", "TM98RNT", 20 },
                    { 8, 2, "Logan", "Dacia", "TM10RNT", 20 },
                    { 9, 1, "Megane", "Renault", "TM15RNT", 22 }
                });

            migrationBuilder.InsertData(
                table: "Minivans",
                columns: new[] { "Id", "PassangersSeatsCount" },
                values: new object[,]
                {
                    { 4, 8 },
                    { 5, 9 },
                    { 6, 6 }
                });

            migrationBuilder.InsertData(
                table: "Sedans",
                columns: new[] { "Id", "Color" },
                values: new object[,]
                {
                    { 7, "Red" },
                    { 8, "White" },
                    { 9, "Black" }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "Id", "CargoCapacity" },
                values: new object[,]
                {
                    { 1, 20 },
                    { 2, 50 },
                    { 3, 40 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DomainId",
                table: "Categories",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PaymentCurrencyId",
                table: "Contracts",
                column: "PaymentCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_RentableItemId",
                table: "Damages",
                column: "RentableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentables_CategoryId",
                table: "Rentables",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentables_DomainId",
                table: "Rentables",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalDamages_DamageId",
                table: "RentalDamages",
                column: "DamageId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalDamages_RentalId",
                table: "RentalDamages",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_ContractId",
                table: "Rentals",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_RentedItemId",
                table: "Rentals",
                column: "RentedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FuelId",
                table: "Vehicles",
                column: "FuelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Minivans");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RentalDamages");

            migrationBuilder.DropTable(
                name: "Sedans");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "VehicleRentals");

            migrationBuilder.DropTable(
                name: "Damages");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Fuels");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Rentables");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Domains");
        }
    }
}
