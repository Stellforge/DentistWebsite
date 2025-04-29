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
    public class DentistManager : ServiceBase<DentistEntity>, IDentistService
    {
        private readonly IUserService _userService;
        private readonly IMediaService _mediaService;
        public DentistManager(IEntityRepository<DentistEntity> repository, IMapper mapper, BaseEntityValidator<DentistEntity> validator, IHttpContextAccessor httpContext, IMediaService mediaService, IUserService userService) : base(repository, mapper, validator, httpContext)
        {
            _mediaService = mediaService;
            _userService = userService;
        }

        public async Task<BussinessLayerResult<DentistListDto>> Add(DentistDto dentist)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<DentistEntity>(dentist);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;
                    dentist.User.Role = Entities.Enum.ERoleType.Dentist;
                    dentist.User.Email = dentist.User.Email??dentist.Email;
                    dentist.Experience = dentist.Experience ?? "";
                    dentist.Explantion = dentist.Explantion ?? "";
                    dentist.User.ProfilePhoto = dentist.User.ProfilePhoto ?? dentist.Photo;
                    var userAddResult = await _userService.Add(dentist.User);
                    if (userAddResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(userAddResult.ErrorMessages);
                        return result;
                    }
                    entity.UserId = userAddResult.Result.Id;

                    if (dentist.Photo != null)
                    {
                        var mediaResult = await _mediaService.Add(new MediaDto { File = dentist.Photo });
                        if (mediaResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                            return result;
                        }
                        entity.PhotoId= mediaResult.Result.Id;
                    }
                    else
                    {
                        scope.Dispose();
                        result.AddError(EErrorCode.DentistDentistAddValidationError, "Ekleme iþlemi sýrasýnda fotoðraf girilmesi zorunludur");
                        return result;  
                    }


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.DentistDentistAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<DentistListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose( );
                    result.AddError(EErrorCode.DentistDentistAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<DentistListDto>> ChangePhoto(DentistDto dentist)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(dentist.Id);
                    if (entity != null)
                    {
                        var mediaResult = (entity.PhotoId != null)
                             ? await _mediaService.Update(new MediaDto { File = dentist.Photo, Id = entity.PhotoId })
                             : await _mediaService.Add(new MediaDto { File = dentist.Photo });
                        if (mediaResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                            return result;
                        }
                            
                    }
                    result.Result = Mapper.Map<DentistListDto>(entity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.DentistDentistDeleteExceptionError, ex.Message);
                }
            }
            return result;
        }

        public async Task<BussinessLayerResult<int>> Count(DentistFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.NameSurname) || (x.User.Name + " " + x.User.Surname).Contains(filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Email) || x.User.Email.Contains(filter.Email))
                 && (string.IsNullOrEmpty(filter.Phone) || x.User.Phone.Contains(filter.Phone))
                 && (string.IsNullOrEmpty(filter.Search) || (x.User.Name + "" + x.User.Surname + "" + x.User.Email + "" + x.User.Phone + "" + x.Title + "" + x.Experience + "" + x.Explantion + "").Contains(filter.Search))
                 && (filter.Gender == null || filter.Gender == x.User.Gender)
                 //&&( x.IsDeleted == false)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistDentistCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.SoftDelete(id);
                    if (entity != null)
                    {
                        var userResult = await _userService.Delete(entity.UserId);
                        if (userResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(userResult.ErrorMessages);
                            return result;
                        }
                    }
                    result.Result = Mapper.Map<DentistListDto>(entity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose( );
                    result.AddError(EErrorCode.DentistDentistDeleteExceptionError, ex.Message);
                }
            }
            return result;



        }

        public async Task<BussinessLayerResult<DentistListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<DentistListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistDentistGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<DentistListDto>>> GetAll(LoadMoreFilter<DentistFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<DentistListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x => 
                    (string.IsNullOrEmpty(filter.Filter.NameSurname) || (x.User.Name + " " + x.User.Surname).Contains(filter.Filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Filter.Email) || x.User.Email.Contains(filter.Filter.Email))
                 && (string.IsNullOrEmpty(filter.Filter.Phone) || x.User.Phone.Contains(filter.Filter.Phone))
                 && (string.IsNullOrEmpty(filter.Filter.Search) || (x.User.Name + ""+x.User.Surname + "" + x.User.Email + "" + x.User.Phone + "" + x.Title + "" + x.Experience + "" + x.Explantion + "").Contains(filter.Filter.Search))
                 && (filter.Filter.Gender == null || filter.Filter.Gender == x.User.Gender)
                 && (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<DentistListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<DentistListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<DentistListDto>
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
                result.AddError(EErrorCode.DentistDentistGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistListDto>> GetByUserId(long userId)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            try
            {
                var entity = await Repository.Get(x=>x.UserId==userId);
                result.Result = Mapper.Map<DentistListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistDentistGetExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistListDto>> Update(DentistDto dentist)
        {
            var result = new BussinessLayerResult<DentistListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(dentist.Id);
                    entity.IsDeleted = false;

                    entity.UpdateTime = DateTime.Now;


                    entity.JobStartDate = dentist.JobStartDate;
                    entity.Title = dentist.Title ?? "";
                    entity.Explantion = dentist.Explantion??"";
                    entity.Experience = dentist.Experience??"";
                    dentist.User.Id = entity.UserId;
                   
                    var userAddResult = await _userService.Update(dentist.User);
                    if (userAddResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(userAddResult.ErrorMessages);
                        return result;
                    }



                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new ErrorDto
                                    {
                                        ErrorCode = EErrorCode.DentistDentistUpdateValidationError,
                                        Message = x.ErrorMessage
                                    }
                                 )
                             );
                        return result;
                    }

                    entity = await Repository.Update(entity);
                    result.Result = Mapper.Map<DentistListDto>(entity);

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    result.AddError(EErrorCode.DentistDentistUpdateExceptionError, ex.Message);

                }
            }
            return result;
        }
    }
}


