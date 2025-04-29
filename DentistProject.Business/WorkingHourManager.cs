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
    public class WorkingHourManager : ServiceBase<WorkingHourEntity>, IWorkingHourService
    {
        public WorkingHourManager(IEntityRepository<WorkingHourEntity> repository, IMapper mapper, BaseEntityValidator<WorkingHourEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<WorkingHourListDto>> AddOrUpdate(WorkingHourDto workinghour)
        {
            var result = new BussinessLayerResult<WorkingHourListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<WorkingHourEntity>(workinghour);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    var oldEntities = await Repository.GetAll(x => !x.IsDeleted && x.DentistId == workinghour.DentistId);
                    foreach (var item in oldEntities)
                    {
                        item.UpdateTime = DateTime.Now;
                        await Repository.SoftDelete(item);
                    }


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.WorkingHourWorkingHourAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<WorkingHourListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose( );
                    result.AddError(EErrorCode.WorkingHourWorkingHourAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(WorkingHourFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  (filter.DentistId == null || filter.DentistId == x.DentistId)
                 && (filter.Monday == null || (x.MondayStart != null || x.MondayEnd != null))
                 && (filter.Sunday == null || (x.SundayStart != null || x.SundayEnd != null))
                 && (filter.Tuesday == null || (x.TuesdayStart != null || x.TuesdayEnd != null))
                 && (filter.Wednesday == null || (x.WednesdayStart != null || x.WednesdayEnd != null))
                 && (filter.Thursday == null || (x.ThursdayStart != null || x.ThursdayEnd != null))
                 && (filter.Friday == null || (x.FridayStart != null || x.FridayEnd != null))
                 && (filter.Saturday == null || (x.SaturdayStart != null || x.SaturdayEnd != null))

                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.WorkingHourWorkingHourCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<WorkingHourListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<WorkingHourListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<WorkingHourListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.WorkingHourWorkingHourDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<WorkingHourListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<WorkingHourListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<WorkingHourListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.WorkingHourWorkingHourGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<WorkingHourListDto>>> GetAll(LoadMoreFilter<WorkingHourFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<WorkingHourListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                     (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                 && (filter.Filter.Monday == null || (x.MondayStart != null || x.MondayEnd != null))
                 && (filter.Filter.Sunday == null || (x.SundayStart != null || x.SundayEnd != null))
                 && (filter.Filter.Tuesday == null || (x.TuesdayStart != null || x.TuesdayEnd != null))
                 && (filter.Filter.Wednesday == null || (x.WednesdayStart != null || x.WednesdayEnd != null))
                 && (filter.Filter.Thursday == null || (x.ThursdayStart != null || x.ThursdayEnd != null))
                 && (filter.Filter.Friday == null || (x.FridayStart != null || x.FridayEnd != null))
                 && (filter.Filter.Saturday == null || (x.SaturdayStart != null || x.SaturdayEnd != null))
                 && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<WorkingHourListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<WorkingHourListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<WorkingHourListDto>
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
                result.AddError(EErrorCode.WorkingHourWorkingHourGetAllExceptionError, ex.Message);
            }
            return result;
        }

    }
}


