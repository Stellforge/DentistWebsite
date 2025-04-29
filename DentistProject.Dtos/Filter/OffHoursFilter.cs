using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public  class OffHoursFilter:FilterBase

    {
        public long?  DentistId { get; set; }
        //public  DateTime? StartHours { get; set; }
        //public DateTime?  EndHours  { get; set; }
        
        

    }
}
