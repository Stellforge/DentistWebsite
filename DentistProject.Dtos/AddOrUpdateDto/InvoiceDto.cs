using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentistProject.Dtos.AddOrUpdateDto;


public class InvoiceDto:DtoBase
{
    public long PatientTreatmentId   { get; set; }
    public long Price { get; set; }
    public long Sale { get; set; }
    public long  EndPrice { get; set; }
    public EPayment PaymentType  { get; set; }

    
    

}
