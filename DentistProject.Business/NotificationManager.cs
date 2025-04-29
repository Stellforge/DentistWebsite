using DentistProject.Business.Abstract;
using DentistProject.Core.ExtensionMethods;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DentistProject.Business
{
    public class NotificationManager : INotificationService
    {
        private readonly ISystemSettingService _systemSettingService;

        public NotificationManager(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        public async Task<BussinessLayerResult<bool>> NotifyUserOnEmail(NotificationDto notification)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var message = notification.Message;


                foreach (var item in notification.Values)
                {
                    message = message.Replace(item.Key, item.Value);

                }

                var smtpResult = await _systemSettingService.GetSmtp();
                if (smtpResult.Status == EResultStatus.Success)
                {
                    new MailSender(smtpResult.Result).SendEmail(notification.Title, message, true,notification.Email);
                }

                response.Result = true;


            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.NotificationNotificationNotifyUserOnEmailExceptionError, ex.Message);
            }

            return response;
        }
    }
}
