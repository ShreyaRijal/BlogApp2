using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp2.Data.Migrations
{
    public partial class CanOnlyLikeOrDislikeOnce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikesOrDislikesBlogs",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    BlogID = table.Column<string>(nullable: false),
                    HasLiked = table.Column<bool>(nullable: false),
                    HasDisliked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesOrDislikesBlogs", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikesOrDislikesBlogs");
        }
    }
}
