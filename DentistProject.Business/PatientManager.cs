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
using static System.Formats.Asn1.AsnWriter;
using System.Transactions;

namespace DentistProject.Business
{
    public class PatientManager : ServiceBase<PatientEntity>, IPatientService
    {
        private readonly IUserService _userService;
        private readonly IMediaService _mediaService;
        public PatientManager(IEntityRepository<PatientEntity> repository, IMapper mapper, BaseEntityValidator<PatientEntity> validator, IHttpContextAccessor httpContext, IUserService userService, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _userService = userService;
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<PatientListDto>> Add(PatientDto patient)
        {
            var result = new BussinessLayerResult<PatientListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<PatientEntity>(patient);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;


                    patient.User.Role = Entities.Enum.ERoleType.Patient;
                    
                    var userAddResult = await _userService.Add(patient.User);
                    if (userAddResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(userAddResult.ErrorMessages);
                        return result;
                    }
                    entity.UserId = userAddResult.Result.Id;

                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.PatientPatientAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<PatientListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.PatientPatientAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(PatientFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                     (string.IsNullOrEmpty(filter.NameSurname) || (x.User.Name + " " + x.User.Surname).Contains(filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Email) || x.User.Email.Contains(filter.Email))
                 && (string.IsNullOrEmpty(filter.Phone) || x.User.Phone.Contains(filter.Phone))
                 && (string.IsNullOrEmpty(filter.IdentityNumber) || x.IdentityNumber.Contains(filter.IdentityNumber))
                 && (string.IsNullOrEmpty(filter.Search) || (x.User.Name + "" + x.User.Surname + "" + x.User.Email + "" + x.User.Phone + "" + x.IdentityNumber + "").Contains(filter.Search))
                 && (filter.Gender == null || filter.Gender == x.User.Gender)


                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPatientCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPatientDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPatientGetExceptionError, ex.Message);
            }
            return result;
        }
        public async Task<BussinessLayerResult<PatientListDto>> GetByUserId(long id)
        {
            var result = new BussinessLayerResult<PatientListDto>();
            try
            {
                var entity = await Repository.Get(x=>x.UserId==id);
                result.Result = Mapper.Map<PatientListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPatientGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientListDto>>> GetAll(LoadMoreFilter<PatientFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.NameSurname) || (x.User.Name + " " + x.User.Surname).Contains(filter.Filter.NameSurname))
                 && (string.IsNullOrEmpty(filter.Filter.Email) || x.User.Email.Contains(filter.Filter.Email))
                 && (string.IsNullOrEmpty(filter.Filter.Phone) || x.User.Phone.Contains(filter.Filter.Phone))
                 && (string.IsNullOrEmpty(filter.Filter.IdentityNumber) || x.IdentityNumber.Contains(filter.Filter.IdentityNumber))
                 && (string.IsNullOrEmpty(filter.Filter.Search) || (x.User.Name + "" + x.User.Surname + "" + x.User.Email + "" + x.User.Phone + "" + x.IdentityNumber + "").Contains(filter.Filter.Search))
                 && (filter.Filter.Gender == null || filter.Filter.Gender == x.User.Gender)


                 && (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientListDto>
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
                result.AddError(EErrorCode.PatientPatientGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientListDto>> Update(PatientDto patient)
        {
            var result = new BussinessLayerResult<PatientListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(patient.Id);
                    entity.IsDeleted = false;

                    entity.UpdateTime = DateTime.Now;


                    entity.IdentityNumber = patient.IdentityNumber;
                  
                    var userUpdateResult = await _userService.Update(patient.User);
                    if (userUpdateResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(userUpdateResult.ErrorMessages);
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
                                        ErrorCode = EErrorCode.PatientPatientUpdateValidationError,
                                        Message = x.ErrorMessage
                                    }
                                 )
                             );
                        return result;
                    }

                    entity = await Repository.Update(entity);
                    result.Result = Mapper.Map<PatientListDto>(entity);

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.PatientPatientUpdateExceptionError, ex.Message);

                }
            }
            return result;
        }
    }
}


