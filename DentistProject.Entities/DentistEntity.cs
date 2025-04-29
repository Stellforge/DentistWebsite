using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Dentist")]
    public class DentistEntity:EntityBase
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Explantion { get; set; }
        public long PhotoId { get; set; }
        public DateTime JobStartDate { get; set; }
        public string Experience { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        [ForeignKey(nameof(PhotoId))]
        public MediaEntity Photo { get; set; }
    }
}
