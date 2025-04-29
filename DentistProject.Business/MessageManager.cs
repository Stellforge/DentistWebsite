using AutoMapper;
using DentistProject.Business.Abstract;
using DentistProject.Core.DataAccess;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.LoadMoreDtos;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Abstract;
using DentistProject.Entities;
using DentistProject.Filters.Filter;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.Error;
using System.Transactions;

namespace DentistProject.Business
{
    public class MessageManager : ServiceBase<MessageEntity>, IMessageService
    {
        private readonly INotificationService _notificationService;
        private readonly IUserRoleService _userRoleService;
        public MessageManager(IEntityRepository<MessageEntity> repository, IMapper mapper, BaseEntityValidator<MessageEntity> validator, IHttpContextAccessor httpContext, INotificationService notificationService, IUserRoleService userRoleService) : base(repository, mapper, validator, httpContext)
        {
            _notificationService = notificationService;
            _userRoleService = userRoleService;
        }

        public async Task<BussinessLayerResult<MessageListDto>> Add(MessageDto message)
        {
            var result = new BussinessLayerResult<MessageListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try
                {
                    var entity = Mapper.Map<MessageEntity>(message);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;




                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.MessageMessageAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<MessageListDto>(entity);

                    var messageReceiversResult = await _userRoleService.GetAll(new LoadMoreFilter<UserRoleFilter>
                    {
                        ContentCount = int.MaxValue,
                        PageCount = 0,
                        Filter = new UserRoleFilter
                        {
                            Role = Entities.Enum.ERoleType.PublicMessageReceiver
                        }
                    });
                    if (messageReceiversResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose ();
                        result.ErrorMessages.AddRange(messageReceiversResult.ErrorMessages);
                        return result;
                    }
                    var messageReceiverEmails=messageReceiversResult.Result.Values?.Select(x=>x.User.Email).ToArray()??new string[0];

                    var notifyResult = await _notificationService.NotifyUserOnEmail(new NotificationDto
                    {
                        Email = messageReceiverEmails,
                        Message = "<!DOCTYPE html>\n<html>\n<head>\n<style>\n\n</style>\n</head>\n<body>\n\n<h1>Yeni bir mesajýnýz var |_Title_|</h1>\n<ul>\n\t<li><p>Ýsim:  |_Name_|</p></li>\n    <li><p>Email:  |_Email_|</p></li>\n    <li><p>Baþlýk\t:  |_Title_|</p></li>\n    <li><p>Mesaj:  |_Message_|</p></li>\n</ul>\n\n<footer>\n\t<h2>|_CompanyName_|</h2>\n</footer>\n\n</body>\n</html>\n\n",
                        Title = "Yeni Bir mesajýnýz var \"|_Title_|\"",
                        Values = new Dictionary<string, string> { { "|_Title_|", entity.Subject }, { "|_Name_|", entity.Name }, { "|_Message_|", entity.Message }, { "|_CompanyName_|", "Dentist Project" }, { "|_Email_|", entity.Email } }
                    });
                    if (notifyResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(notifyResult.ErrorMessages);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    result.AddError(EErrorCode.MessageMessageAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(MessageFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Search) || (x.Subject + " " + x.Message + " " + x.Name + " " + x.Email + " " ).Contains(filter.Search))
                 && (filter.MaxDate == null || filter.MaxDate >= x.Date)
                 && (filter.MinDate == null || filter.MinDate <= x.Date)
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.MessageMessageCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<MessageListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<MessageListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<MessageListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.MessageMessageDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<MessageListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<MessageListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<MessageListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.MessageMessageGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<MessageListDto>>> GetAll(LoadMoreFilter<MessageFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<MessageListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                         (string.IsNullOrEmpty(filter.Filter.Search) || (x.Subject + " " + x.Message + " " + x.Name + " " + x.Email + " ").Contains(filter.Filter.Search))
                 && (filter.Filter.MaxDate == null || filter.Filter.MaxDate >= x.Date)
                 && (filter.Filter.MinDate == null || filter.Filter.MinDate <= x.Date)
                    && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<MessageListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<MessageListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<MessageListDto>
                {
                    Values = values,
                    ContentCount = filter.ContentCount,
                    NextPage = lastIndex < entities.Count,
                    TotalPageCount = Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount)),
                    TotalContentCount = entities.Count,
                    PageCount = filter.PageCount > Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount))
                    ? Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount))
                    : filter.PageCount,
                    PrevPage = firstIndex > 0


                };

            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.MessageMessageGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<MessageListDto>> Update(MessageDto message)
        {
            var result = new BussinessLayerResult<MessageListDto>();
            try
            {
                var entity = await Repository.Get(message.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;



                entity.Status = message.Status;
                entity.Subject = message.Subject;
                entity.Message= message.Message;
                entity.Email = message.Email;
                entity.Date = DateTime.Now;
                entity.Name = message.Name;

                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.MessageMessageUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<MessageListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.MessageMessageUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


