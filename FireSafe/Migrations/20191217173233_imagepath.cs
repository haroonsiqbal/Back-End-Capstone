using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSafe.Migrations
{
    public partial class imagepath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_UploadImages_UploadImageImageId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UploadImageImageId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UploadImageImageId",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "UploadImageImageId",
                table: "Logs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UploadImageImageId",
                table: "Logs",
                column: "UploadImageImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_UploadImages_UploadImageImageId",
                table: "Logs",
                column: "UploadImageImageId",
                principalTable: "UploadImages",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
