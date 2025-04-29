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
using static System.Formats.Asn1.AsnWriter;

namespace DentistProject.Business
{
    public class UserManager : ServiceBase<UserEntity>, IUserService
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRoleService _roleService;
        private readonly IMediaService _mediaService;
        public UserManager(IEntityRepository<UserEntity> repository, IMapper mapper, BaseEntityValidator<UserEntity> validator, IHttpContextAccessor httpContext, IIdentityService identityService, IUserRoleService roleService, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _identityService = identityService;
            _roleService = roleService;
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<UserListDto>> Add(UserDto user)
        {
            var result = new BussinessLayerResult<UserListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    user.Address = user.Address ?? "";
                    
                    var entity = Mapper.Map<UserEntity>(user);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    
                   


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.UserUserAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );

                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<UserListDto>(entity);
                    //identity add
                    var identityResult = await _identityService.Add(new IdentityDto
                    {
                        Password = user.Password,
                        UserId = entity.Id,
                    });
                    if (identityResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(identityResult.ErrorMessages);
                        return result;
                    }
                    // user role add
                    var roleResult = await _roleService.Add(new UserRoleDto
                    {
                        Role = user.Role,
                        UserId = entity.Id,
                    });
                    if (roleResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(roleResult.ErrorMessages);
                        return result;
                    }
                    // profile photo add
                    if (user.ProfilePhoto != null)
                    {

                        var profileMediaResult = await _mediaService.Add(new MediaDto
                        {
                            File = user.ProfilePhoto
                        });
                        if (profileMediaResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(profileMediaResult.ErrorMessages);
                            return result;
                        }
                    }
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.UserUserAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<UserListDto>> ChangePhoto(UserDto user)
        {
            var result = new BussinessLayerResult<UserListDto>();
            try
            {
                var entity = await Repository.Get(user.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                if (user.ProfilePhoto != null)
                {
                    var profileMediaResult = (entity.ProfilePhotoId != null)
                        ? await _mediaService.Update(new MediaDto { File = user.ProfilePhoto, Id = (long)entity.ProfilePhotoId })
                        : await _mediaService.Add(new MediaDto { File = user.ProfilePhoto });
                    
                    if (profileMediaResult.Status == EResultStatus.Error)
                    {
                        result.ErrorMessages.AddRange(profileMediaResult.ErrorMessages.ToList());
                        return result;
                    }

                    entity.ProfilePhotoId = profileMediaResult.Result.Id;

                }
                else
                {
                    entity.ProfilePhotoId = null;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<UserListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserUserChangePhotoExceptionError, ex.Message);

            }
            return result;
        }

        public async Task<BussinessLayerResult<int>> Count(UserFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.NameSurname) || (x.Name + " " + x.Surname).Contains(filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Email) || x.Email.Contains(filter.Email))
                 && (string.IsNullOrEmpty(filter.Phone) || x.Phone.Contains(filter.Phone))
                 && (filter.Gender == null || filter.Gender == x.Gender)

                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserUserCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<UserListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<UserListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<UserListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserUserDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<UserListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<UserListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<UserListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserUserGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<UserListDto>>> GetAll(LoadMoreFilter<UserFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<UserListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.NameSurname) || (x.Name + " " + x.Surname).Contains(filter.Filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Filter.Email) || x.Email.Contains(filter.Filter.Email))
                 && (string.IsNullOrEmpty(filter.Filter.Phone) || x.Phone.Contains(filter.Filter.Phone))
                 && (filter.Filter.Gender == null || filter.Filter.Gender == x.Gender)
                 && (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<UserListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<UserListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<UserListDto>
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
                result.AddError(EErrorCode.UserUserGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<UserListDto>> Update(UserDto user)
        {
            var result = new BussinessLayerResult<UserListDto>();
            try
            {
                var entity = await Repository.Get(user.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.BirthDate = user.BirthDate;
                entity.Name = user.Name;
                entity.Surname = user.Surname;
                entity.Email = user.Email;
                entity.Phone = user.Phone;
                entity.Address = user.Address??"";
                entity.Gender = user.Gender;



                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.UserUserUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<UserListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserUserUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


