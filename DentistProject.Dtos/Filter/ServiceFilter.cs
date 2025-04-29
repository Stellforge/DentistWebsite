using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class ServiceFilter:FilterBase
    {
        public string? Search { get; set; }
        public Decimal?  MinPrice { get; set; }
        public Decimal?  MaxPrice { get; set; }

        
        

    }
}
