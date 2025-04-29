using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_raport_treatment_delete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientReport_PatientTreatment_TreatmentId",
                table: "PatientReport");

            migrationBuilder.DropIndex(
                name: "IX_PatientReport_TreatmentId",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "PatientReport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TreatmentId",
                table: "PatientReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PatientReport_TreatmentId",
                table: "PatientReport",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientReport_PatientTreatment_TreatmentId",
                table: "PatientReport",
                column: "TreatmentId",
                principalTable: "PatientTreatment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
