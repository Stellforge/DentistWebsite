using DentistProject.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("AppointmentRequest")]
    public class AppointmentRequestEntity : EntityBase
    {
        public long DentistId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public long PatientId { get; set; }
        public string Message { get; set; }

        [ForeignKey(nameof(DentistId))]
        public DentistEntity Dentist { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity Patient { get; set; }




    }
}
