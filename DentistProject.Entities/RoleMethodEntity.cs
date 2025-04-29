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
    [Table("RoleMethod")]
    public class RoleMethodEntity:EntityBase
    {
        public ERoleType Role { get; set; }
        public EMethod Method { get; set; }


        
    }
}
