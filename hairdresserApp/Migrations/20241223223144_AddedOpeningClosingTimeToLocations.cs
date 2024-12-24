using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairdresserApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedOpeningClosingTimeToLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingTime",
                table: "Locations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningTime",
                table: "Locations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Locations");
        }
    }
}
