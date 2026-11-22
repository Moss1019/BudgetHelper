using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetHelper_ef.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePKNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "Receipts",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Receipts",
                newName: "ReceiptId");
        }
    }
}
