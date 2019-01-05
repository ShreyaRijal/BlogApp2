using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp2.Data.Migrations
{
    public partial class LikesAndDislikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlogCommentedOnID",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfDislikes",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfLikes",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfDislikes",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "NumOfLikes",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "BlogCommentedOnID",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
