using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentistProject.Filters.Filter;


public class InvoiceFilter:FilterBase
{
    public long? PatientTreatmentId   { get; set; }
    public long?  MinEndPrice { get; set; }
    public long?  MaxEndPrice { get; set; }
    public EPayment? PaymentType  { get; set; }

    
    

}
