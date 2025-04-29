using DentistProject.Entities.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    public class UserRoleEntity:EntityBase
    {
        public long UserId { get; set; }
        public ERoleType Role { get; set; }





        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
    }
}
