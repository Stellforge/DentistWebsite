using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    [Table ("PatientPrescriptionMedicine")]
    public class PatientPrescriptionMedicineDto:DtoBase
    {
        public long PatientPrescriptionId { get; set; }
        public string  Medicine { get; set; }
        
        
    }
}
