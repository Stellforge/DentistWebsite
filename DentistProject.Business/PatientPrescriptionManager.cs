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
    public class PatientPrescriptionManager : ServiceBase<PatientPrescriptionEntity>, IPatientPrescriptionService
    {
        public PatientPrescriptionManager(IEntityRepository<PatientPrescriptionEntity> repository, IMapper mapper, BaseEntityValidator<PatientPrescriptionEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<PatientPrescriptionListDto>> Add(PatientPrescriptionDto patientprescription)
        {
            var result = new BussinessLayerResult<PatientPrescriptionListDto>();
            try
            {
                var entity = Mapper.Map<PatientPrescriptionEntity>(patientprescription);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientPrescriptionPatientPrescriptionAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<PatientPrescriptionListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(PatientPrescriptionFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                 (filter.DentistId == null || filter.DentistId == x.DentistId)
                 &&(filter.PatientId == null || filter.PatientId == x.PatientId)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientPrescriptionListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientPrescriptionListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientPrescriptionListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientPrescriptionListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientPrescriptionListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientPrescriptionListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionListDto>>> GetAll(LoadMoreFilter<PatientPrescriptionFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                 && (filter.Filter.PatientId == null || filter.Filter.PatientId == x.PatientId)
                 && (filter.Filter.DentistUserId == null || filter.Filter.DentistUserId == x.Dentist.UserId)
                 && (filter.Filter.Search == null || (x.Dentist.User.Name + " " + x.Dentist.User.Surname + " " + x.Patient.User.Name + " " + x.Patient.User.Surname + " " + x.Patient.User.Phone + " " + x.Patient.IdentityNumber + " " ).Contains(filter.Filter.Search))
                 && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientPrescriptionListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientPrescriptionListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientPrescriptionListDto>
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
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientPrescriptionListDto>> Update(PatientPrescriptionDto patientprescription)
        {
            var result = new BussinessLayerResult<PatientPrescriptionListDto>();
            try
            {
                var entity = await Repository.Get(patientprescription.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.DentistId = patientprescription.DentistId;
                entity.PatientId = patientprescription.PatientId;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientPrescriptionPatientPrescriptionUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<PatientPrescriptionListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionPatientPrescriptionUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


