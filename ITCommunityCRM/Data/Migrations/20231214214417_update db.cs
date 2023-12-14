using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITCommunityCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tags_TagId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tags_TagId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Tags_TagId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Tags_TagId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_TagId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TagId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Events_TagId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TagId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "NotificationMessageTemplateId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EventGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventGroup_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGroup_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSpeaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SpeakerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSpeaker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSpeaker_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSpeaker_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTag_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUser_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupTag_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeakerTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    SpeakerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeakerTag_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeakerTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTag_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGroup_EventId",
                table: "EventGroup",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventGroup_GroupId",
                table: "EventGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeaker_EventId",
                table: "EventSpeaker",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeaker_SpeakerId",
                table: "EventSpeaker",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_EventId",
                table: "EventTag",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_UserId",
                table: "EventTag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUser_EventId",
                table: "EventUser",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUser_UserId",
                table: "EventUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTag_GroupId",
                table: "GroupTag",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTag_TagId",
                table: "GroupTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerTag_SpeakerId",
                table: "SpeakerTag",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerTag_TagId",
                table: "SpeakerTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTag_TagId",
                table: "UserTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTag_UserId",
                table: "UserTag",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGroup");

            migrationBuilder.DropTable(
                name: "EventSpeaker");

            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.DropTable(
                name: "EventUser");

            migrationBuilder.DropTable(
                name: "GroupTag");

            migrationBuilder.DropTable(
                name: "SpeakerTag");

            migrationBuilder.DropTable(
                name: "UserTag");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Speakers");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Speakers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationMessageTemplateId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_TagId",
                table: "Speakers",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TagId",
                table: "Groups",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TagId",
                table: "Events",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TagId",
                table: "AspNetUsers",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tags_TagId",
                table: "AspNetUsers",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tags_TagId",
                table: "Events",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Tags_TagId",
                table: "Groups",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Tags_TagId",
                table: "Speakers",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
