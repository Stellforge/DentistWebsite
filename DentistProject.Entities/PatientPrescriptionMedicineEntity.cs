using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table ("PatientPrescriptionMedicine")]
    public class PatientPrescriptionMedicineEntity:EntityBase
    {
        public long PatientPrescriptionId { get; set; }
        public string  Medicine { get; set; }
        [ForeignKey(nameof(PatientPrescriptionId))]
        public PatientPrescriptionEntity PatientPrescription { get; set; }
    }
}
