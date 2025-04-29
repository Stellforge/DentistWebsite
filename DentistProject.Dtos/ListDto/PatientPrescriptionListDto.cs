using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class PatientPrescriptionListDto : DtoBase
    {
        public long DentistId { get; set; }
        public long PatientId { get; set; }

        
        public DentistListDto Dentist { get; set; }
        
        public PatientListDto Patient { get; set; }
    }
}
