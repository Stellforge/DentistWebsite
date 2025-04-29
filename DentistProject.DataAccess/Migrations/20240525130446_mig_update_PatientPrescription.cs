using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_update_PatientPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientPrescriptionMedicine_Patient_PatientPrescriptionId",
                table: "PatientPrescriptionMedicine");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPrescriptionMedicine_PatientPrescription_PatientPrescriptionId",
                table: "PatientPrescriptionMedicine",
                column: "PatientPrescriptionId",
                principalTable: "PatientPrescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientPrescriptionMedicine_PatientPrescription_PatientPrescriptionId",
                table: "PatientPrescriptionMedicine");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPrescriptionMedicine_Patient_PatientPrescriptionId",
                table: "PatientPrescriptionMedicine",
                column: "PatientPrescriptionId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
