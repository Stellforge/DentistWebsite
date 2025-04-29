using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("PatientPrescription")]
    public class PatientPrescriptionEntity : EntityBase
    {
        public long DentistId { get; set; }
        public long PatientId { get; set; }

        [ForeignKey(nameof(DentistId))]
        public DentistEntity Dentist { get; set; }
        [ForeignKey(nameof(PatientId))]
        public PatientEntity Patient { get; set; }
    }
}
