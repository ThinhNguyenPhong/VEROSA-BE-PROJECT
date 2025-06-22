using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VEROSA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateDbv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Addresses",
                newName: "District");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Accounts",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Addresses",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
