using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    

    public class PatientTreatmentDto:DtoBase

    {

        public long PatientId { get; set; }
        public long InterveningDentistId { get; set; }
        public string Explanation { get; set; }

        

        
        

    }
}
