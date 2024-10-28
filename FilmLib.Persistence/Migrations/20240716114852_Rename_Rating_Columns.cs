using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmLib.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Rating_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingObject_RatingValue",
                table: "Films",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "RatingObject_NumberOfVotes",
                table: "Films",
                newName: "NumberOfVotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Films",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingObject_RatingValue",
                table: "Actors",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Films",
                newName: "RatingObject_RatingValue");

            migrationBuilder.RenameColumn(
                name: "NumberOfVotes",
                table: "Films",
                newName: "RatingObject_NumberOfVotes");

            migrationBuilder.AlterColumn<double>(
                name: "RatingObject_RatingValue",
                table: "Films",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "RatingObject_RatingValue",
                table: "Actors",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
