using DentistProject.Dtos.Enum;
using DentistProject.Dtos.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.Result
{
    public class BussinessLayerResult<T>
    {
        public T Result { get; set; }
        public List<ErrorDto>? ErrorMessages { get; set; }
        public EResultStatus Status
        {
            get
            {
                return (ErrorMessages?.Count > 0) ? EResultStatus.Error : EResultStatus.Success;
            }
        }
        public BussinessLayerResult()
        {
            ErrorMessages = new List<ErrorDto>();
        }

        public void AddError(EErrorCode errorCode, string message = "")
        {
            if (ErrorMessages == null)
            {
                ErrorMessages = new List<ErrorDto>();
            }
            ErrorMessages.Add(new ErrorDto { ErrorCode = errorCode, Message = message });

        }

    }
}
