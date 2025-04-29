using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class PatientReportListDto:DtoBase
    {
        public string Title { get; set; }
        public string Explanation { get; set; }
        public long FileId { get; set; }
        public long PatientId { get; set; }
        public string ReportType { get; set; }
        public long? TreatmentId { get; set; }
        
        public MediaListDto File { get; set; }
        
        public PatientListDto Patient { get; set; }
        
        public PatientTreatmentListDto? Treatment  {get; set; }


    }
}
