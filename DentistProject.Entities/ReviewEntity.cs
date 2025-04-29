using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Review")]
   public class ReviewEntity:EntityBase
    {
        public string Review { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
    }
}
