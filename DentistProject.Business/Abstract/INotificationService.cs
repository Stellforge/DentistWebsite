using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Abstract
{
    public interface INotificationService
    {
        public Task<BussinessLayerResult<bool>> NotifyUserOnEmail(NotificationDto notification);
    }
}
