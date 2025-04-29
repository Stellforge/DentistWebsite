using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class IdentityCheckDto : DtoBase
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
        public EDeviceType DeviceType { get; set; }
        public bool RememberMe { get; set; }
    }
}
