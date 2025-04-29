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
    public class AppointmentManager : ServiceBase<AppointmentEntity>, IAppointmentService
    {
        public AppointmentManager(IEntityRepository<AppointmentEntity> repository, IMapper mapper, BaseEntityValidator<AppointmentEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<AppointmentListDto>> Add(AppointmentDto appointment, bool force)
        {
            var result = new BussinessLayerResult<AppointmentListDto>();
            try
            {
                var entity = Mapper.Map<AppointmentEntity>(appointment);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;
                //Todo: Ýzin saatlerine göre kontrol eklenecek
                if (!force)
                {

                    //var offdays=await _offHoursService.Count(new OffHoursFilter
                    //{
                    //    DentistId
                    //})




                    var otherAppoiments = await Repository.GetAll(x => x.DentistId == appointment.DentistId  && (x.AppointmentValidity == Entities.Enum.EAppointmentValidity.Valid)
                    && (
                       (x.InspectionDate < appointment.InspectionDate && (x.InspectionDate.AddHours(x.InspectionTimeHour)) > appointment.InspectionDate)
                       || (x.InspectionDate < (appointment.InspectionDate + appointment.InspectionTime) && (x.InspectionDate.AddHours(x.InspectionTimeHour)) > (appointment.InspectionDate + appointment.InspectionTime))
                       || (x.InspectionDate > appointment.InspectionDate && (x.InspectionDate.AddHours(x.InspectionTimeHour)) < (appointment.InspectionDate + appointment.InspectionTime))

                   )
                   && (x.IsDeleted == false)
                   );
                    if (otherAppoiments.Count > 0)
                    {
                        result.AddError(EErrorCode.AppointmentAppointmentAddExceptionError, "There is another appointment within the specified date range");
                        return result;
                    }
                }



                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.AppointmentAppointmentAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<AppointmentListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentAppointmentAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(AppointmentFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                 (filter.AppointmentType == null || filter.AppointmentType == x.AppointmentType)
                 && (filter.AppointmentValidity == null || filter.AppointmentValidity == x.AppointmentValidity)
                 && (filter.DentistId == null || filter.DentistId == x.DentistId)
                 && (filter.PatientId == null || filter.PatientId == x.PatientId)
                 && (filter.InspectionMaxDate == null || filter.InspectionMaxDate >= x.InspectionDate)
                 && (filter.InspectionMinDate == null || filter.InspectionMinDate <= x.InspectionDate)

                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentAppointmentCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AppointmentListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<AppointmentListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<AppointmentListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentAppointmentDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<AppointmentListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<AppointmentListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<AppointmentListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentAppointmentGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<AppointmentListDto>>> GetAll(LoadMoreFilter<AppointmentFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<AppointmentListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.AppointmentType == null || filter.Filter.AppointmentType == x.AppointmentType)
                 && (filter.Filter.AppointmentValidity == null || filter.Filter.AppointmentValidity == x.AppointmentValidity)
                 && (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                 && (filter.Filter.PatientId == null || filter.Filter.PatientId == x.PatientId)
                 && (filter.Filter.InspectionMaxDate == null || filter.Filter.InspectionMaxDate >= x.InspectionDate)
                 && (filter.Filter.InspectionMinDate == null || filter.Filter.InspectionMinDate <= x.InspectionDate)
                 && (filter.Filter.UserId == null || filter.Filter.UserId == x.Dentist.UserId)
                &&(x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<AppointmentListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<AppointmentListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<AppointmentListDto>
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
                result.AddError(EErrorCode.AppointmentAppointmentGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AppointmentListDto>> Update(AppointmentDto appointment,bool force)
        {
            var result = new BussinessLayerResult<AppointmentListDto>();
            try
            {
                if (!force)
                {
                    
                    var otherAppoiments = await Repository.GetAll(x => x.DentistId == appointment.DentistId &&( x.Id!=appointment.Id)  &&(x.AppointmentValidity==Entities.Enum.EAppointmentValidity.Valid)
                    && (
                       (x.InspectionDate < appointment.InspectionDate && (x.InspectionDate.AddHours(x.InspectionTimeHour)) > appointment.InspectionDate)
                       || (x.InspectionDate < (appointment.InspectionDate + appointment.InspectionTime) && (x.InspectionDate.AddHours(x.InspectionTimeHour)) > (appointment.InspectionDate + appointment.InspectionTime))
                       || (x.InspectionDate > appointment.InspectionDate && (x.InspectionDate.AddHours(x.InspectionTimeHour)) < (appointment.InspectionDate + appointment.InspectionTime))

                   )
                   && (x.IsDeleted == false)
                   );
                    if (otherAppoiments.Count > 0)
                    {
                        result.AddError(EErrorCode.AppointmentAppointmentAddExceptionError, "There is another appointment within the specified date range");
                        return result;
                    }
                }

                var entity = await Repository.Get(appointment.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;



                entity.InspectionDate=appointment.InspectionDate;
                entity.AppointmentType = appointment.AppointmentType;
                entity.DentistId=appointment.DentistId;
                entity.PatientId = appointment.PatientId;
                entity.InspectionTimeHour = appointment.InspectionTimeHour;
                entity.AppointmentValidity = appointment.AppointmentValidity;
                entity.Origin = appointment.Origin;
               

                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.AppointmentAppointmentUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<AppointmentListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AppointmentAppointmentUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


