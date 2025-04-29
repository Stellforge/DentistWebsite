using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Patient")]
    public class PatientEntity : EntityBase

    {
        public long UserId { get; set; }
        public string IdentityNumber { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
      
    }
}
