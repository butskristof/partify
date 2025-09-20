using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Partify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpotifyIdLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SpotifyTokens",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(22)",
                oldFixedLength: true,
                oldMaxLength: 22);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SpotifyTokens",
                type: "character(22)",
                fixedLength: true,
                maxLength: 22,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
