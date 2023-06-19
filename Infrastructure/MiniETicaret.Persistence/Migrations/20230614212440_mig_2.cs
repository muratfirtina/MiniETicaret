using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniETicaret.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CartItemId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_CartItemId",
                table: "Files",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_CartItems_CartItemId",
                table: "Files",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_CartItems_CartItemId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CartItemId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "Files");
        }
    }
}
