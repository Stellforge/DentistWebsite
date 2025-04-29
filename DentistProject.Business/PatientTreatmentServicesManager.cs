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

namespace DentistProject.Business
{
    public class PatientTreatmentServicesManager : ServiceBase<PatientTreatmentServicesEntity>, IPatientTreatmentServicesService
    {
        public PatientTreatmentServicesManager(IEntityRepository<PatientTreatmentServicesEntity> repository, IMapper mapper, BaseEntityValidator<PatientTreatmentServicesEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Add(PatientTreatmentServicesDto patienttreatmentservices)
        {
            var result = new BussinessLayerResult<PatientTreatmentServicesListDto>();
            try
            {
                var entity = Mapper.Map<PatientTreatmentServicesEntity>(patienttreatmentservices);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientTreatmentServicesPatientTreatmentServicesAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<PatientTreatmentServicesListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(PatientTreatmentServicesFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                    (filter.SeviceId == null || filter.SeviceId == x.ServiceId)
                 && (filter.PatientTreatmentId == null || filter.PatientTreatmentId == x.PatientTreatmentId)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientTreatmentServicesListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientTreatmentServicesListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientTreatmentServicesListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientTreatmentServicesListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentServicesListDto>>> GetAll(LoadMoreFilter<PatientTreatmentServicesFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentServicesListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (filter.Filter.SeviceId == null || filter.Filter.SeviceId == x.ServiceId)
                 && (filter.Filter.PatientTreatmentId == null || filter.Filter.PatientTreatmentId == x.PatientTreatmentId)
                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientTreatmentServicesListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientTreatmentServicesListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientTreatmentServicesListDto>
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
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Update(PatientTreatmentServicesDto patienttreatmentservices)
        {
            var result = new BussinessLayerResult<PatientTreatmentServicesListDto>();
            try
            {
                var entity = await Repository.Get(patienttreatmentservices.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.ServiceId = patienttreatmentservices.ServiceId;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientTreatmentServicesPatientTreatmentServicesUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<PatientTreatmentServicesListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentServicesPatientTreatmentServicesUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


