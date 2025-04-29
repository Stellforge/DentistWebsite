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
    public class ServiceManager : ServiceBase<ServiceEntity>, IServiceService
    {
        private readonly IMediaService _mediaService;
        public ServiceManager(IEntityRepository<ServiceEntity> repository, IMapper mapper, BaseEntityValidator<ServiceEntity> validator, IHttpContextAccessor httpContext, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<ServiceListDto>> Add(ServiceDto service)
        {
            var result = new BussinessLayerResult<ServiceListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<ServiceEntity>(service);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    var mediaResult = await _mediaService.Add(new MediaDto { File = service.Logo });
                    if (mediaResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                        return result;
                    }
                    entity.LogoId = mediaResult.Result.Id;


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.ServiceServiceAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<ServiceListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.ServiceServiceAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<ServiceListDto>> ChangeLogo(ServiceDto service)
        {
            var result = new BussinessLayerResult<ServiceListDto>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(service.Id);
                    if (entity != null)
                    {
                        var mediaResult = (entity.LogoId != null)
                             ? await _mediaService.Update(new MediaDto { File = service.Logo, Id = entity.LogoId })
                             : await _mediaService.Add(new MediaDto { File = service.Logo });
                        if (mediaResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                            return result;
                        }

                    }
                    result.Result = Mapper.Map<ServiceListDto>(entity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.ServiceServiceDeleteExceptionError, ex.Message);
                }
            }
            return result;
        }

        public async Task<BussinessLayerResult<int>> Count(ServiceFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Search) || (x.Title + " " + x.Explanation).Contains(filter.Search))
                 && (filter.MaxPrice == null || filter.MaxPrice >= x.Price)
                 && (filter.MinPrice == null || filter.MinPrice <= x.Price)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ServiceServiceCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ServiceListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<ServiceListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<ServiceListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ServiceServiceDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<ServiceListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<ServiceListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<ServiceListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ServiceServiceGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<ServiceListDto>>> GetAll(LoadMoreFilter<ServiceFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<ServiceListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                 (string.IsNullOrEmpty(filter.Filter.Search) || (x.Title + " " + x.Explanation).Contains(filter.Filter.Search))
                 && (filter.Filter.MaxPrice == null || filter.Filter.MaxPrice >= x.Price)
                 && (filter.Filter.MinPrice == null || filter.Filter.MinPrice <= x.Price)
                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<ServiceListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<ServiceListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<ServiceListDto>
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
                result.AddError(EErrorCode.ServiceServiceGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ServiceListDto>> Update(ServiceDto service)
        {
            var result = new BussinessLayerResult<ServiceListDto>();
            try
            {
                var entity = await Repository.Get(service.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Title = service.Title;
                entity.Price = service.Price;
                entity.Explanation = service.Explanation;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.ServiceServiceUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<ServiceListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ServiceServiceUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


