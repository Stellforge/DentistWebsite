using DentistProject.Entities.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentistProject.Entities;

[Table("Invoice")]
public class InvoiceEntity:EntityBase
{
    public long PatientTreatmentId   { get; set; }
    public long Price { get; set; }
    public long Sale { get; set; }
    public long  EndPrice { get; set; }
    public EPayment PaymentType  { get; set; }

    [ForeignKey(nameof(PatientTreatmentId))]
    public PatientTreatmentEntity PatientTreatment { get; set; }

}
