using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("PatientTreatmentServices")]
    public class PatientTreatmentServicesEntity:EntityBase
    {
        public long PatientTreatmentId   { get; set; }
        public long  ServiceId { get; set; }
        [ForeignKey(nameof (PatientTreatmentId))] 
        public PatientEntity PatientTreatment { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public ServiceEntity Service { get; set; }


    }
}
