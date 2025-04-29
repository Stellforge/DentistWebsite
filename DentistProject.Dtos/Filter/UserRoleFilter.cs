using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    public class UserRoleFilter:FilterBase
    {
        public long? UserId { get; set; }
        public ERoleType? Role { get; set; }








    }
}
