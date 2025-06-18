using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VEROSA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateDBv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Accounts",
                type: "datetime(6)",
                maxLength: 50,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Accounts");
        }
    }
}
