using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Service")]
    public class ServiceEntity:EntityBase
    {
        public string Title { get; set; }
        public long  LogoId { get; set; }
        public string Explanation { get; set; }
        public Decimal  Price { get; set; }

        [ForeignKey(nameof(LogoId))]
        public MediaEntity Logo { get; set; }

    }
}
