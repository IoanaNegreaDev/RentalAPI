using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalAPI.Migrations
{
    public partial class RemovedRentalDamage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalDamages");

            migrationBuilder.AddColumn<int>(
                name: "OccuredInRentalId",
                table: "Damages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Damages_OccuredInRentalId",
                table: "Damages",
                column: "OccuredInRentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_Rentals",
                table: "Damages",
                column: "OccuredInRentalId",
                principalTable: "Rentals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Damages_Rentals",
                table: "Damages");

            migrationBuilder.DropIndex(
                name: "IX_Damages_OccuredInRentalId",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "OccuredInRentalId",
                table: "Damages");

            migrationBuilder.CreateTable(
                name: "RentalDamages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamageId = table.Column<int>(type: "int", nullable: false),
                    RentalId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_RentalDamages_DamageId",
                table: "RentalDamages",
                column: "DamageId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalDamages_RentalId",
                table: "RentalDamages",
                column: "RentalId");
        }
    }
}
