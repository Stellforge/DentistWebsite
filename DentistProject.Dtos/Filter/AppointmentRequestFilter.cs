using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class AppointmentRequestFilter : FilterBase
    {
        public long? DentistId { get; set; }
        public long? DentistUserId { get; set; }

        public long? PatientId { get; set; }

        
        

        
        




    }
}
