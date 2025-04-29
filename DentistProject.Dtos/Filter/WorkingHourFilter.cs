using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public  class WorkingHourFilter:FilterBase

    {
        public long? DentistId { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get;set; }
        public bool? Thursday { get; private set; }
        public bool? Friday { get; internal set; }
        public bool? Saturday { get; protected set; }
        public bool? Sunday { get; protected set; }

        
        

    }
}
