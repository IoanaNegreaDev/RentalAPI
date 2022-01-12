using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalAPI.Migrations
{
    public partial class RemovedDomainFromRentable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentables_Domains_DomainId",
                table: "Rentables");

            migrationBuilder.DropIndex(
                name: "IX_Rentables_DomainId",
                table: "Rentables");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Rentables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Rentables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentables_DomainId",
                table: "Rentables",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentables_Domains_DomainId",
                table: "Rentables",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
