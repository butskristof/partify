using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Partify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotifyTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.CreateTable(
                name: "SpotifyTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character(22)", fixedLength: true, maxLength: 22, nullable: false),
                    AccessToken = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    AccessTokenExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyTokens", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyTokens");
        }
    }
}
