using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkPulse.Api.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedConcurrencyProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Polls");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Polls",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Polls");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Polls",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
