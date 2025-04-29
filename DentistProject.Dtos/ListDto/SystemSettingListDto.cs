using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    public class SystemSettingListDto:DtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ESettingKey Key { get; set; }
        public string Value { get; set; }
    }
}
