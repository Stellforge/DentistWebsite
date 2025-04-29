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
    public class PatientPrescriptionMedicineManager : ServiceBase<PatientPrescriptionMedicineEntity>, IPatientPrescriptionMedicineService
    {
        public PatientPrescriptionMedicineManager(IEntityRepository<PatientPrescriptionMedicineEntity> repository, IMapper mapper, BaseEntityValidator<PatientPrescriptionMedicineEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Add(PatientPrescriptionMedicineDto patientprescriptionmedicine)
        {
            var result = new BussinessLayerResult<PatientPrescriptionMedicineListDto>();
            try
            {
                var entity = Mapper.Map<PatientPrescriptionMedicineEntity>(patientprescriptionmedicine);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<PatientPrescriptionMedicineListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(PatientPrescriptionMedicineFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Medicine) || x.Medicine.Contains(filter.Medicine))
                 && (filter.PatientPrescriptionId == null || filter.PatientPrescriptionId == x.PatientPrescriptionId)
                 
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientPrescriptionMedicineListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientPrescriptionMedicineListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientPrescriptionMedicineListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientPrescriptionMedicineListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionMedicineListDto>>> GetAll(LoadMoreFilter<PatientPrescriptionMedicineFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionMedicineListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.Medicine) || x.Medicine.Contains(filter.Filter.Medicine))
                 && (filter.Filter.PatientPrescriptionId == null || filter.Filter.PatientPrescriptionId == x.PatientPrescriptionId)

                && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientPrescriptionMedicineListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientPrescriptionMedicineListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientPrescriptionMedicineListDto>
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
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Update(PatientPrescriptionMedicineDto patientprescriptionmedicine)
        {
            var result = new BussinessLayerResult<PatientPrescriptionMedicineListDto>();
            try
            {
                var entity = await Repository.Get(patientprescriptionmedicine.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Medicine=patientprescriptionmedicine.Medicine;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<PatientPrescriptionMedicineListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientPrescriptionMedicinePatientPrescriptionMedicineUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


