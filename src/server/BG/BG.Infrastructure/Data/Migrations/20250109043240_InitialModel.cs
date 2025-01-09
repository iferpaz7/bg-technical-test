using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Config");

            migrationBuilder.CreateTable(
                name: "IdentificationType",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    IdCard = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    IdentificationTypeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(27)", unicode: false, maxLength: 27, nullable: false),
                    FullName = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_IdentificationType_IdentificationTypeId",
                        column: x => x.IdentificationTypeId,
                        principalSchema: "Config",
                        principalTable: "IdentificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_IdentificationTypeCode",
                schema: "Config",
                table: "IdentificationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdentificationTypeId",
                schema: "Config",
                table: "Person",
                column: "IdentificationTypeId");

            migrationBuilder.CreateIndex(
                name: "UX_PersonCode",
                schema: "Config",
                table: "Person",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_PersonIdCard",
                schema: "Config",
                table: "Person",
                column: "IdCard",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Username",
                schema: "Config",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "IdentificationType",
                schema: "Config");
        }
    }
}
