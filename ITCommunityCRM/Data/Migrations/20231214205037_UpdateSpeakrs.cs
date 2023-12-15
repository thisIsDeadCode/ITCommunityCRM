using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpeakrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Speakers");
        }
    }
}
