using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("PatientReport")]
    public class PatientReportEntity:EntityBase
    {
        public string Title { get; set; }
        public string Explanation { get; set; }
        public long FileId { get; set; }
        public long PatientId { get; set; }
        public string ReportType { get; set; }
        //public long? TreatmentId { get; set; }
        [ForeignKey(nameof(FileId))]
        public MediaEntity File { get; set; }
        [ForeignKey(nameof(PatientId))]
        public PatientEntity Patient { get; set; }
        //[ForeignKey(nameof(TreatmentId))]
        //public PatientTreatmentEntity? Treatment  {get; set; }


    }
}
