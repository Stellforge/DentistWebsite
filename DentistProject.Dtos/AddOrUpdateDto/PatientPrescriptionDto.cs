using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class PatientPrescriptionDto : DtoBase
    {
        public long DentistId { get; set; }
        public long PatientId { get; set; }

        
        
        
        
    }
}
