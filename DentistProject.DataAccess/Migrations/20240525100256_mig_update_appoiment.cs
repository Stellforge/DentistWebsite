using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_update_appoiment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectionTime",
                table: "Appointment");

            migrationBuilder.AddColumn<float>(
                name: "InspectionTimeHour",
                table: "Appointment",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectionTimeHour",
                table: "Appointment");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "InspectionTime",
                table: "Appointment",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
