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
using DentistProject.Core.ExtensionMethods;
using System.Security.Principal;

namespace DentistProject.Business
{
    public class IdentityManager : ServiceBase<IdentityEntity>, IIdentityService
    {
        private readonly INotificationService _notificationService;
        public IdentityManager(IEntityRepository<IdentityEntity> repository, IMapper mapper, BaseEntityValidator<IdentityEntity> validator, IHttpContextAccessor httpContext, INotificationService notificationService) : base(repository, mapper, validator, httpContext)
        {
            _notificationService = notificationService;
        }

        public async Task<BussinessLayerResult<IdentityListDto>> Add(IdentityDto identity)
        {
            var result = new BussinessLayerResult<IdentityListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var oldEntities = await Repository.GetAll(x => x.UserId == identity.UserId && (x.ExpiryDate == null || x.ExpiryDate > DateTime.Now) && x.IsDeleted == false);
                    foreach (var item in oldEntities)
                    {
                        await Repository.SoftDelete(item.Id);

                    }
                    var salt = ExtensionsMethods.GenerateRandomString(128);
                    var entity = new IdentityEntity
                    {
                        CreateTime = DateTime.Now,
                        UserId = identity.UserId,
                        ExpiryDate = null,
                        IsDeleted = false,
                        PasswordSalt = salt,
                        PasswordHash = ExtensionsMethods.CalculateMD5Hash(salt + identity.Password + salt),

                    };


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.IdentityIdentityAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<IdentityListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.IdentityIdentityAddExceptionError, ex.Message);
                }
            }
            return result;

        }


        public async Task<BussinessLayerResult<IdentityListDto>> CheckPassword(IdentityCheckDto identity)
        {
            var result = new BussinessLayerResult<IdentityListDto>();
            try
            {
                var entities = await Repository.GetAll(x =>
                (x.User.Email.Equals(identity.Email)||x.User.Phone.Equals(identity.Email))
                && (x.ExpiryDate == null || x.ExpiryDate > DateTime.Now)
                && x.IsDeleted == false
                && x.User.IsDeleted==false
                 );

                entities = entities.Where(x => x.PasswordHash == ExtensionsMethods.CalculateMD5Hash(x.PasswordSalt + identity.Password + x.PasswordSalt)).ToList();
                if (entities.Count > 0)
                {
                    result.Result = Mapper.Map<IdentityListDto>(entities[0]);
                    return result;
                }
                result.AddError(EErrorCode.IdentityIdentityCheckExceptionError, "Username or password is wrong");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.IdentityIdentityGetExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<IdentityListDto>> ForgatPassword(long userId)
        {
            var response = new BussinessLayerResult<IdentityListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var oldEntities = await Repository.GetAll(x => (x.ExpiryDate==null || x.ExpiryDate>DateTime.Now) && x.UserId == userId);
                    oldEntities.ForEach(x =>
                    {
                        x.ExpiryDate = DateTime.Now;
                        x.UpdateTime = DateTime.Now;
                        Repository.Update(x);
                    });
                    var password = ExtensionsMethods.GenerateRandomPassword(10);
                    var addResponse = await Add(new IdentityDto
                    {
                        Password = password,
                        UserId = userId,
                    });

                    if (addResponse.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        response.ErrorMessages.AddRange(addResponse.ErrorMessages);
                        return response;
                    }

                    var identity= await Repository.Get(addResponse.Result.Id);

                    var notificationResult = await _notificationService.NotifyUserOnEmail(new NotificationDto
                    {
                        Title = "Þifreniz Deðiþtirilmiþtir",
                        Email=[identity.User.Email],
                        Message=$"<h1><strong>Dentist Project </strong></h1><h2><strong><pre> < Dentist Project > </pre></strong></h2>" +
                    $" <pre>Þifreniz {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} te deðiþtirilmiþtir.  \n" +
                    $"Yeni Þifreniz:{password} \n" +
                    $"Lütfen þifrenizi kimse ile paylaþmayýnýz. </pre>",
                        Values= new Dictionary<string, string>()
                    }
                    );
                    if (notificationResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        response.ErrorMessages.AddRange(notificationResult.ErrorMessages);
                        return response;
                    }

                    response.Result = addResponse.Result;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    response.AddError(EErrorCode.IdentityIdentityForgatExceptionError, ex.Message);
                }
            }
            return response;
        }



        public async Task<BussinessLayerResult<IdentityListDto>> ChangePassword(IdentityDto identity)
        {
            var result = await Add(identity);
            return result;
        }
    }
}


