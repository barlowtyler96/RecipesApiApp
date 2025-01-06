using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeLibraryEF.Migrations
{
    public partial class AddCreatedByForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CreatedBySub",
                table: "Recipes",
                column: "CreatedBySub");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_CreatedBySub",
                table: "Recipes",
                column: "CreatedBySub",
                principalTable: "Users",
                principalColumn: "Sub",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_CreatedBySub",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CreatedBySub",
                table: "Recipes");
        }
    }
}
