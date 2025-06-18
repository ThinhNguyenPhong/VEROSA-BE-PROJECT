using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VEROSA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Accounts",
                    type: "varchar(20)",
                    maxLength: 20,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .AlterColumn<string>(
                    name: "PasswordHash",
                    table: "Accounts",
                    type: "text",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "longtext"
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Accounts",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldMaxLength: 50
            );

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationToken",
                table: "Accounts",
                type: "varchar(255)",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationTokenExpires",
                table: "Accounts",
                type: "datetime(6)",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Accounts",
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(20)",
                    oldMaxLength: 20
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .AlterColumn<string>(
                    name: "PasswordHash",
                    table: "Accounts",
                    type: "longtext",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "text"
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Accounts",
                type: "datetime(6)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmationTokenExpires",
                table: "Accounts",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true
            );

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "ConfirmationToken",
                keyValue: null,
                column: "ConfirmationToken",
                value: ""
            );

            migrationBuilder
                .AlterColumn<string>(
                    name: "ConfirmationToken",
                    table: "Accounts",
                    type: "longtext",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(255)",
                    oldUnicode: false,
                    oldMaxLength: 255,
                    oldNullable: true
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
