using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class PatientTreatmentServicesFilter:FilterBase
    {
        public long? PatientTreatmentId   { get; set; }
        public long?  SeviceId { get; set; }
        

        
        


    }
}
