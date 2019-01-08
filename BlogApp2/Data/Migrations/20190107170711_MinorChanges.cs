using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp2.Data.Migrations
{
    public partial class MinorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "Comments",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "Comments",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 400);
        }
    }
}
