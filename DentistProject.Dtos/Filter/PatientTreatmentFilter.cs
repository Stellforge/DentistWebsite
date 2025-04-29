using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    

    public class PatientTreatmentFilter:FilterBase

    {

        public long? PatientId { get; set; }
        public long? InterveningDentistId { get; set; }

        

        
        

    }
}
