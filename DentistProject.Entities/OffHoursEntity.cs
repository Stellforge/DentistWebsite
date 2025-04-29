using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Offhours")]
    public  class OffHoursEntity:EntityBase

    {
        public long  DentistId { get; set; }
        public  DateTime StartHours { get; set; }
        public DateTime  EndHours  { get; set; }
        [ForeignKey(nameof(DentistId))]
        public DentistEntity Dentist { get; set; }

    }
}
