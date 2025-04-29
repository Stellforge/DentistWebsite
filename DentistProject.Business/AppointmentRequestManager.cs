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
    public class AppointmentRequestManager : ServiceBase<AppointmentRequestEntity>, IAppointmentRequestService
    {
        private readonly IPatientService _patientService;
        public AppointmentRequestManager(IEntityRepository<AppointmentRequestEntity> repository, IMapper mapper, BaseEntityValidator<AppointmentRequestEntity> validator, IHttpContextAccessor httpContext, IPatientService patientService) : base(repository, mapper, validator, httpContext)
        {
            _patientService = patientService;
        }

        public async Task<BussinessLayerResult<AppointmentRequestListDto>> Add(AppointmentRequestDto appointmentrequest)
        {
            var result = new BussinessLayerResult<AppointmentRequestListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<AppointmentRequestEntity>(appointmentrequest);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    if (entity.PatientId == 0)
                    {
                        var patientResult = await _patientService.Add(appointmentrequest.Patient);
                        if(patientResult.Status==EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(patientResult.ErrorMessages);
                            return result;
                        }
                        entity.PatientId = patientResult.Result.Id;
                    }


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.AppointmentRequestAppointmentRequestAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<AppointmentRequestListDto>(entity);

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose( );
                    result.AddError(EErrorCode.AppointmentRequestAppointmentRequestAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(AppointmentRequestFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                 (filter.DentistId == null || filter.DentistId == x.DentistId)
                 && (filter.PatientId == null || filter.PatientId == x.PatientId)
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentRequestAppointmentRequestCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AppointmentRequestListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<AppointmentRequestListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<AppointmentRequestListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentRequestAppointmentRequestDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<AppointmentRequestListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<AppointmentRequestListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<AppointmentRequestListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentRequestAppointmentRequestGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<AppointmentRequestListDto>>> GetAll(LoadMoreFilter<AppointmentRequestFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<AppointmentRequestListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                    && (filter.Filter.PatientId == null || filter.Filter.PatientId == x.PatientId)
                    && (filter.Filter.DentistUserId == null || filter.Filter.DentistUserId == x.Dentist.UserId)
                &&(x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<AppointmentRequestListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<AppointmentRequestListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<AppointmentRequestListDto>
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
                result.AddError(EErrorCode.AppointmentRequestAppointmentRequestGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AppointmentRequestListDto>> Update(AppointmentRequestDto appointmentrequest)
        {
            var result = new BussinessLayerResult<AppointmentRequestListDto>();
            try
            {
                var entity = await Repository.Get(appointmentrequest.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.StartTime=appointmentrequest.StartTime;
                entity.FinishTime = appointmentrequest.FinishTime;
                entity.DentistId = appointmentrequest.DentistId;
                entity.Message = appointmentrequest.Message;
                
                
                var patientResult= await _patientService.GetByUserId(appointmentrequest.UserId);
                if (patientResult.Status==EResultStatus.Error)
                {
                    result.ErrorMessages.AddRange(patientResult.ErrorMessages);
                    return result;
                }

                if (patientResult.Result == null)
                {
                    patientResult= await _patientService.Add(appointmentrequest.Patient);
                    if(patientResult.Status==EResultStatus.Error)
                    {
                        result.ErrorMessages.AddRange(patientResult.ErrorMessages);
                        return result;
                    }
                }

                entity.PatientId = patientResult.Result.Id;
                

                //Todo: Yukarý kýsým özel olarak test edilecek Amaç bir randevu isteði oluþturulurken bir hasta da oluþtururlmasý eðer yoksa


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.AppointmentRequestAppointmentRequestUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<AppointmentRequestListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentRequestAppointmentRequestUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


