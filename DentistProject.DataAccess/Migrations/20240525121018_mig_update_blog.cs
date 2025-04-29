using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_update_blog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_DentistSocial_CategoryId",
                table: "Blog");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_BlogCategory_CategoryId",
                table: "Blog",
                column: "CategoryId",
                principalTable: "BlogCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_BlogCategory_CategoryId",
                table: "Blog");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_DentistSocial_CategoryId",
                table: "Blog",
                column: "CategoryId",
                principalTable: "DentistSocial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
