using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    [Table ("PatientPrescriptionMedicine")]
    public class PatientPrescriptionMedicineListDto:DtoBase
    {
        public long PatientPrescriptionId { get; set; }
        public string  Medicine { get; set; }
        
        public PatientPrescriptionListDto PatientPrescription { get; set; }
    }
}
