using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_user_table_and_add_group_table_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_NotificationTypes_NotificationTypeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Telegram",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NotificationTypeId",
                table: "Events",
                newName: "NotificationTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_NotificationTypeId",
                table: "Events",
                newName: "IX_Events_NotificationTemplateId");

            migrationBuilder.AddColumn<int>(
                name: "NotificationTypeId",
                table: "NotificationMessageTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NotificationMessageTemplateId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupIdCode = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "NotificationMessageTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessageTemplates_NotificationTypeId",
                table: "NotificationMessageTemplates",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_NotificationMessageTemplates_NotificationTemplateId",
                table: "Events",
                column: "NotificationTemplateId",
                principalTable: "NotificationMessageTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessageTemplates_NotificationTypes_NotificationTypeId",
                table: "NotificationMessageTemplates",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_NotificationMessageTemplates_NotificationTemplateId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessageTemplates_NotificationTypes_NotificationTypeId",
                table: "NotificationMessageTemplates");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMessageTemplates_NotificationTypeId",
                table: "NotificationMessageTemplates");

            migrationBuilder.DropColumn(
                name: "NotificationTypeId",
                table: "NotificationMessageTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NotificationMessageTemplateId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NotificationTemplateId",
                table: "Events",
                newName: "NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_NotificationTemplateId",
                table: "Events",
                newName: "IX_Events_NotificationTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_NotificationTypes_NotificationTypeId",
                table: "Events",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
