using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class MessageEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMembers_Routes_RouteId",
                table: "ChannelMembers");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ChannelId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMembers_RouteId",
                table: "ChannelMembers");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "MessageHeaders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ChannelId",
                table: "Routes",
                column: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Routes_ChannelId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "MessageHeaders");

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_ChannelMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ChannelMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ChannelId",
                table: "Routes",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMembers_RouteId",
                table: "ChannelMembers",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MemberId",
                table: "Reactions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId",
                table: "Reactions",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMembers_Routes_RouteId",
                table: "ChannelMembers",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id");
        }
    }
}
