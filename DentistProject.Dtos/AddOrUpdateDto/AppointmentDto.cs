using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{

    public class AppointmentDto : DtoBase
    {
        public long DentistId { get; set; }
        public DateTime InspectionDate { get; set; }
        public string? Origin { get; set; }
        public EAppointmentValidity AppointmentValidity { get; set; }

        public long PatientId { get; set; }

        public EAppointmentType AppointmentType { get; set; }


        public float InspectionTimeHour { get; set; }

        [NotMapped]
        public TimeSpan InspectionTime
        {
            get
            {

                return TimeSpan.FromHours(InspectionTimeHour);

            }
        }
       


    }
}
