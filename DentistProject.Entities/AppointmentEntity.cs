using DentistProject.Entities.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Appointment")]
    public class AppointmentEntity:EntityBase
    {
        public long DentistId { get; set; }
        public DateTime InspectionDate { get; set; }
        public string? Origin { get; set; }
        public EAppointmentValidity AppointmentValidity { get; set; }

        public long PatientId { get; set; }

        public EAppointmentType AppointmentType { get; set; }


        public float InspectionTimeHour { get; set; }

        [ForeignKey(nameof(DentistId))]
        public DentistEntity Dentist { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity Patient { get; set; }

        [NotMapped]
        public TimeSpan InspectionTime { get; private set; }
        public AppointmentEntity()
        {
            InspectionTime = TimeSpan.FromHours(InspectionTimeHour);
        }
    }
}
