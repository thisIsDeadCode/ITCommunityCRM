using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_bd_7788 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationMessageTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "NotificationTemplateTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationMessageTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Title");
        }
    }
}
