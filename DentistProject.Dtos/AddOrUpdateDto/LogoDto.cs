using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class LogoDto : SystemSettingDto
    {
        public IFormFile File { get; set; }
    }
}
