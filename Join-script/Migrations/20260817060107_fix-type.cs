using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Join_script.Migrations
{
    /// <inheritdoc />
    public partial class fixtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Properties_TypeId",
                table: "Properties",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_TypeId",
                table: "Properties",
                column: "TypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_TypeId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_TypeId",
                table: "Properties");
        }
    }
}
