using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSafe.Migrations
{
    public partial class images2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UploadImageImageId",
                table: "Logs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UploadImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageFile = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    ImageDescription = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_UploadImages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UploadImageImageId",
                table: "Logs",
                column: "UploadImageImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadImages_UserId",
                table: "UploadImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_UploadImages_UploadImageImageId",
                table: "Logs",
                column: "UploadImageImageId",
                principalTable: "UploadImages",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_UploadImages_UploadImageImageId",
                table: "Logs");

            migrationBuilder.DropTable(
                name: "UploadImages");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UploadImageImageId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UploadImageImageId",
                table: "Logs");
        }
    }
}
