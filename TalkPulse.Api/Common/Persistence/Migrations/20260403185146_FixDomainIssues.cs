using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkPulse.Api.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDomainIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_AudienceFingerprint",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_AudienceFingerprint",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_SessionId",
                table: "Feedbacks");

            migrationBuilder.AddColumn<Guid>(
                name: "PollId",
                table: "Votes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "PollOptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PollId_AudienceFingerprint",
                table: "Votes",
                columns: new[] { "PollId", "AudienceFingerprint" },
                unique: true,
                filter: "\"AudienceFingerprint\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SessionId_AudienceFingerprint",
                table: "Feedbacks",
                columns: new[] { "SessionId", "AudienceFingerprint" },
                unique: true,
                filter: "\"AudienceFingerprint\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Polls_PollId",
                table: "Votes",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Polls_PollId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_PollId_AudienceFingerprint",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_SessionId_AudienceFingerprint",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "PollId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "PollOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_AudienceFingerprint",
                table: "Votes",
                column: "AudienceFingerprint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AudienceFingerprint",
                table: "Feedbacks",
                column: "AudienceFingerprint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SessionId",
                table: "Feedbacks",
                column: "SessionId");
        }
    }
}
