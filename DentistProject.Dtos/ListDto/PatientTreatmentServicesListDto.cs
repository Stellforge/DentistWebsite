using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class PatientTreatmentServicesListDto:DtoBase
    {
        public long PatientTreatmentId   { get; set; }
        public long  ServiceId { get; set; }
        [ForeignKey(nameof (PatientTreatmentId))] 
        public PatientListDto PatientTreatment { get; set; }

        
        public ServiceListDto Service { get; set; }


    }
}
