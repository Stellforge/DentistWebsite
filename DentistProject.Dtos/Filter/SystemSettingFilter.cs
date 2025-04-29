using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    public class SystemSettingFilter:FilterBase
    {

        public ESettingKey? Key { get; set; }
    }
}
