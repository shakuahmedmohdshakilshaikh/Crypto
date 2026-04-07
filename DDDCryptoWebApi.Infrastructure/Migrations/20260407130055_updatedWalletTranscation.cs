using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDCryptoWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedWalletTranscation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "WalletTransactions");
        }
    }
}
