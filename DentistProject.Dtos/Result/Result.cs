using DentistProject.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.Result
{
    public class Result<T>
    {
        public T Value { get; set; }
        public EResultStatus ResultStatus
        {
            get
            {

                return (Message == null || Message.Count() == 0) ? EResultStatus.Success : EResultStatus.Error;
            }
        }
        public string[]? Message { get; set; }
    }
}
