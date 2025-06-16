using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TradeAppDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Migration_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "UserSymbols");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Portfolios");

            migrationBuilder.RenameColumn(
                name: "AssetName",
                table: "Portfolios",
                newName: "Symbol");

            migrationBuilder.AddColumn<int>(
                name: "SymbolId",
                table: "UserSymbols",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    SymbolId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.SymbolId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSymbols_SymbolId",
                table: "UserSymbols",
                column: "SymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSymbols_Symbols_SymbolId",
                table: "UserSymbols",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "SymbolId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSymbols_Symbols_SymbolId",
                table: "UserSymbols");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_UserSymbols_SymbolId",
                table: "UserSymbols");

            migrationBuilder.DropColumn(
                name: "SymbolId",
                table: "UserSymbols");

            migrationBuilder.RenameColumn(
                name: "Symbol",
                table: "Portfolios",
                newName: "AssetName");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "UserSymbols",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Portfolios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
