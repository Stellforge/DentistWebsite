using DentistProject.Dtos.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class PatientReportDto:DtoBase
    {
        public string Title { get; set; }
        public string Explanation { get; set; }
        public long PatientId { get; set; }
        public string ReportType { get; set; }
        public long TreatmentId { get; set; }

        public IFormFile File { get; set; }
        
        
        
       


    }
}
