using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    [Table ("PatientPrescriptionMedicine")]
    public class PatientPrescriptionMedicineFilter:FilterBase
    {
        public long? PatientPrescriptionId { get; set; }
        public string?  Medicine { get; set; }
        
        
    }
}
