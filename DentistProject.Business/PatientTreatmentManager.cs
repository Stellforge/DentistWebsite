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
    public class PatientTreatmentManager : ServiceBase<PatientTreatmentEntity>, IPatientTreatmentService
    {
        public PatientTreatmentManager(IEntityRepository<PatientTreatmentEntity> repository, IMapper mapper, BaseEntityValidator<PatientTreatmentEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<PatientTreatmentListDto>> Add(PatientTreatmentDto patienttreatment)
        {
            var result = new BussinessLayerResult<PatientTreatmentListDto>();
            try
            {
                var entity = Mapper.Map<PatientTreatmentEntity>(patienttreatment);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientTreatmentPatientTreatmentAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<PatientTreatmentListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(PatientTreatmentFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  (filter.PatientId == null || filter.PatientId == x.PatientId)
                 && (filter.InterveningDentistId == null || filter.InterveningDentistId == x.InterveningDentistId)
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientTreatmentListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientTreatmentListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientTreatmentListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientTreatmentListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientTreatmentListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientTreatmentListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentListDto>>> GetAll(LoadMoreFilter<PatientTreatmentFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                     (filter.Filter.PatientId == null || filter.Filter.PatientId == x.PatientId)
                 && (filter.Filter.InterveningDentistId == null || filter.Filter.InterveningDentistId == x.InterveningDentistId)
                    && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientTreatmentListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientTreatmentListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientTreatmentListDto>
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
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientTreatmentListDto>> Update(PatientTreatmentDto patienttreatment)
        {
            var result = new BussinessLayerResult<PatientTreatmentListDto>();
            try
            {
                var entity = await Repository.Get(patienttreatment.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.InterveningDentistId = entity.InterveningDentistId;
                entity.Explanation= patienttreatment.Explanation;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientTreatmentPatientTreatmentUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<PatientTreatmentListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientTreatmentPatientTreatmentUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


