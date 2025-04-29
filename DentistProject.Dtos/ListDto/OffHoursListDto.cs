using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public  class OffHoursListDto:DtoBase

    {
        public long  DentistId { get; set; }
        public  DateTime StartHours { get; set; }
        public DateTime  EndHours  { get; set; }
        
        public DentistListDto Dentist { get; set; }

    }
}
