using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp2.Data.Migrations
{
    public partial class AddLikesOrDislikesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LikesOrDislikesBlogs",
                table: "LikesOrDislikesBlogs");

            migrationBuilder.AlterColumn<string>(
                name: "BlogID",
                table: "LikesOrDislikesBlogs",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LikesOrDislikesBlogs_BlogID_UserName",
                table: "LikesOrDislikesBlogs",
                columns: new[] { "BlogID", "UserName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikesOrDislikesBlogs",
                table: "LikesOrDislikesBlogs",
                columns: new[] { "UserName", "BlogID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_LikesOrDislikesBlogs_BlogID_UserName",
                table: "LikesOrDislikesBlogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikesOrDislikesBlogs",
                table: "LikesOrDislikesBlogs");

            migrationBuilder.AlterColumn<string>(
                name: "BlogID",
                table: "LikesOrDislikesBlogs",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikesOrDislikesBlogs",
                table: "LikesOrDislikesBlogs",
                column: "UserName");
        }
    }
}
