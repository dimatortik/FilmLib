using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmLib.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating_RatingValue",
                table: "Films",
                newName: "RatingObject_RatingValue");

            migrationBuilder.RenameColumn(
                name: "Rating_NumberOfVotes",
                table: "Films",
                newName: "RatingObject_NumberOfVotes");

            migrationBuilder.RenameColumn(
                name: "Rating_RatingValue",
                table: "Actors",
                newName: "RatingObject_RatingValue");

            migrationBuilder.RenameColumn(
                name: "Rating_NumberOfVotes",
                table: "Actors",
                newName: "RatingObject_NumberOfVotes");

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RatingNumber = table.Column<decimal>(type: "numeric", nullable: false),
                    FilmId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_FilmId",
                table: "Ratings",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.RenameColumn(
                name: "RatingObject_RatingValue",
                table: "Films",
                newName: "Rating_RatingValue");

            migrationBuilder.RenameColumn(
                name: "RatingObject_NumberOfVotes",
                table: "Films",
                newName: "Rating_NumberOfVotes");

            migrationBuilder.RenameColumn(
                name: "RatingObject_RatingValue",
                table: "Actors",
                newName: "Rating_RatingValue");

            migrationBuilder.RenameColumn(
                name: "RatingObject_NumberOfVotes",
                table: "Actors",
                newName: "Rating_NumberOfVotes");
        }
    }
}
