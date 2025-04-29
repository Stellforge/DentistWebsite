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
    public class OffHoursManager : ServiceBase<OffHoursEntity>, IOffHoursService
    {
        private readonly IAppointmentService _appointmentService;
        public OffHoursManager(IEntityRepository<OffHoursEntity> repository, IMapper mapper, BaseEntityValidator<OffHoursEntity> validator, IHttpContextAccessor httpContext, IAppointmentService appointmentService) : base(repository, mapper, validator, httpContext)
        {
            _appointmentService = appointmentService;
        }

        public async Task<BussinessLayerResult<OffHoursListDto>> Add(OffHoursDto offhours,bool force)
        {
            var result = new BussinessLayerResult<OffHoursListDto>();
            try
            {
                var entity = Mapper.Map<OffHoursEntity>(offhours);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;


                if (!force)
                {
                   var appoimentResult= await _appointmentService.Count(new AppointmentFilter
                    {
                        AppointmentValidity = Entities.Enum.EAppointmentValidity.Valid,
                        InspectionMinDate = offhours.StartHours,
                        InspectionMaxDate = offhours.EndHours,
                        DentistId=offhours.DentistId,
                    });

                    if(appoimentResult.Status==EResultStatus.Error)
                    {
                        result.ErrorMessages.AddRange(appoimentResult.ErrorMessages);
                        return result;
                    }


                    if (appoimentResult.Result > 0)
                    {
                        result.AddError(EErrorCode.OffHoursOffHoursAddOneOrMoreAppoimentFoundError, "Belirlenen tarih aralýðýnda bir yada daha fazla randevunuz bulunmaktadýr.");
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
                                    ErrorCode = EErrorCode.OffHoursOffHoursAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<OffHoursListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.OffHoursOffHoursAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(OffHoursFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.DentistId == null || filter.DentistId== x.DentistId)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.OffHoursOffHoursCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<OffHoursListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<OffHoursListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<OffHoursListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.OffHoursOffHoursDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<OffHoursListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<OffHoursListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<OffHoursListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.OffHoursOffHoursGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<OffHoursListDto>>> GetAll(LoadMoreFilter<OffHoursFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<OffHoursListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                    && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<OffHoursListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<OffHoursListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<OffHoursListDto>
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
                result.AddError(EErrorCode.OffHoursOffHoursGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<OffHoursListDto>> Update(OffHoursDto offhours)
        {
            var result = new BussinessLayerResult<OffHoursListDto>();
            try
            {
                var entity = await Repository.Get(offhours.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.StartHours = offhours.StartHours;
                entity.EndHours = offhours.EndHours;
                entity.DentistId = offhours.DentistId;
               

                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.OffHoursOffHoursUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<OffHoursListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.OffHoursOffHoursUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


