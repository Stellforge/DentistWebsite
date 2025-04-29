using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("PatientTreatment")]

    public class PatientTreatmentEntity:EntityBase

    {

        public long PatientId { get; set; }
        public long InterveningDentistId { get; set; }
        public string Explanation { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity Patient {get; set;}

        [ForeignKey(nameof(InterveningDentistId))]
        public DentistEntity InterveningDentist { get; set; }

    }
}
