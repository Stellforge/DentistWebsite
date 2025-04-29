using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_update_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatmentServices_Dentist_SeviceId",
                table: "PatientTreatmentServices");

            migrationBuilder.RenameColumn(
                name: "SeviceId",
                table: "PatientTreatmentServices",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTreatmentServices_SeviceId",
                table: "PatientTreatmentServices",
                newName: "IX_PatientTreatmentServices_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatmentServices_Service_ServiceId",
                table: "PatientTreatmentServices",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatmentServices_Service_ServiceId",
                table: "PatientTreatmentServices");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "PatientTreatmentServices",
                newName: "SeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTreatmentServices_ServiceId",
                table: "PatientTreatmentServices",
                newName: "IX_PatientTreatmentServices_SeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatmentServices_Dentist_SeviceId",
                table: "PatientTreatmentServices",
                column: "SeviceId",
                principalTable: "Dentist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
