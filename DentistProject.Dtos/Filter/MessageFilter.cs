using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class MessageFilter:FilterBase
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string? Search { get; set; }


    }
}
