using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CdPo.Web.Migrations
{
    /// <inheritdoc />
    public partial class FilesAddCachedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Persons_PersonId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "CachedName",
                table: "Files",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CachedName",
                table: "Files");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Persons_PersonId",
                table: "Staffs",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
