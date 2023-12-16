using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addflagtotrackusereventvisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisitedEvent",
                table: "EventUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisitedEvent",
                table: "EventUser");
        }
    }
}
