using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmLib.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_RatingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating_NumberOfVotes",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Rating_RatingValue",
                table: "Films",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Rating_NumberOfVotes",
                table: "Actors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Rating_RatingValue",
                table: "Actors",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating_NumberOfVotes",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Rating_RatingValue",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Rating_NumberOfVotes",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "Rating_RatingValue",
                table: "Actors");
        }
    }
}
