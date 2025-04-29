using DentistProject.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.Error
{
    public class ErrorDto
    {
        public string? Message { get; set; }
        public EErrorCode ErrorCode { get; set; }
    }
}
