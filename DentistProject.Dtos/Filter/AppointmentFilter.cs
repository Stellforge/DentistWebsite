using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class AppointmentFilter:FilterBase
    {
        public long? DentistId { get; set; }
        public long? UserId { get; set; }
        public DateTime? InspectionMinDate { get; set; }
        public DateTime? InspectionMaxDate { get; set; }
    
        public EAppointmentValidity? AppointmentValidity { get; set; }

        public long? PatientId { get; set; }

        public EAppointmentType? AppointmentType { get; set; }

        
        

        
        

    }
}
