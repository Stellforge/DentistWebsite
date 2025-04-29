using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class RoleMethodAllUpdateDto
    {
        public ERoleType Role { get; set; }
        public List<EMethod> Methods { get; set; }
    }
}
