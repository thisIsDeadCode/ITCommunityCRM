using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_isvisited1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
               name: "IsVisitedEvent",
               table: "EventUsers",
               type: "bit",
               nullable: false,
               defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "IsVisitedEvent",
               table: "EventUsers");
        }
    }
}
