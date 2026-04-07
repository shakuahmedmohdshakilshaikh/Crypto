using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDCryptoWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Portfolios",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");

            migrationBuilder.CreateTable(
                name: "PortfolioTransactions",
                columns: table => new
                {
                    PortfolioTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CryptoId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioTransactions", x => x.PortfolioTransactionId);
                    table.ForeignKey(
                        name: "FK_PortfolioTransactions_Cryptos_CryptoId",
                        column: x => x.CryptoId,
                        principalTable: "Cryptos",
                        principalColumn: "CryptoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioTransactions_CryptoId",
                table: "PortfolioTransactions",
                column: "CryptoId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioTransactions_UserId",
                table: "PortfolioTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioTransactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Portfolios",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");
        }
    }
}
