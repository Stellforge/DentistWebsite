using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class PatientTreatmentServicesDto:DtoBase
    {
        public long PatientTreatmentId   { get; set; }
        public long  ServiceId { get; set; }
        

        
        


    }
}
