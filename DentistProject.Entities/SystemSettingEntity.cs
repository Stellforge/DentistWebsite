using DentistProject.Entities.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    public class SystemSettingEntity:EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ESettingKey Key { get; set; }
        public string Value { get; set; }
    }
}
