using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class PatientReportFilter:FilterBase
    {
        public string? Search { get; set; }
        public long? PatientId { get; set; }
        public string? ReportType { get; set; }
        public long? TreatmentId { get; set; }
        
        
        
        
        


    }
}
