using DentistProject.Business.Abstract;
using DentistProject.Core.DataAccess;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Filters;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.LoadMoreDtos;
using DentistProject.Dtos.Result;
using DentistProject.Entities;
using DentistProject.Entities.Abstract;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.Enum;
using Microsoft.AspNetCore.Http;

namespace DentistProject.Business
{
    public class SessionManager : ServiceBase<SessionEntity>, ISessionService
    {
        public SessionManager(IEntityRepository<SessionEntity> repository, IMapper mapper, BaseEntityValidator<SessionEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<SessionListDto>> Add(SessionDto session)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var entity = Mapper.Map<SessionEntity>(session);
                entity.CreateTime = DateTime.Now;
                entity.IsDeleted = false;
                entity.Id = 0;

                var validationResult = Validator.Validate(entity);
                if (!validationResult.IsValid)
                {
                    foreach (var item in validationResult.Errors)
                    {
                        response.AddError(EErrorCode.SessionSessionAddValidationError, item.ErrorMessage);

                    }
                    return response;
                }

                response.Result = Mapper.Map<SessionListDto>(await Repository.Add(entity));

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SessionSessionAddExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<SessionListDto>> Delete(long id)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                await Repository.SoftDelete(id);

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SessionSessionDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SessionListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var entity =await Repository.Get(id);
                var dto = Mapper.Map<SessionListDto>(entity);
                response.Result = dto;

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SessionSessionGetExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SessionListDto>> Get(string key)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var entity = await Repository.Get(x=>x.Key==key&&(x.ExpiryDate>DateTime.Now || x.ExpiryDate==null));
                var dto = Mapper.Map<SessionListDto>(entity);
                response.Result = dto;

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SessionSessionGetExceptionError, ex.Message);
            }
            return response;
        }


        public async Task<BussinessLayerResult<GenericLoadMoreDto<SessionListDto>>> GetAll(LoadMoreFilter<SessionFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<SessionListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                  await  Repository.GetAll(x =>
                (string.IsNullOrEmpty(filter.Filter.IpAddress) || x.IpAddress.Contains(filter.Filter.IpAddress))
                &&(string.IsNullOrEmpty(filter.Filter.Key) || x.Key.Contains(filter.Filter.Key))
                && (filter.Filter.DeviceType == null || filter.Filter.DeviceType == x.DeviceType)
                && (filter.Filter.UserId == null || filter.Filter.UserId == x.UserId)
                && (filter.Filter.MaxExpiryDate == null || filter.Filter.MaxExpiryDate >= x.ExpiryDate)
                && (x.IsDeleted == false)
                ) :await Repository.GetAll(x => x.IsDeleted == false);

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<SessionListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<SessionListDto>(entities[i]));
                }

                response.Result = new GenericLoadMoreDto<SessionListDto>
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
                response.AddError(EErrorCode.SessionSessionGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SessionListDto>> Update(SessionDto session)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var entity = await Repository.Get(session.Id);
                entity.Key= session.Key;
                entity.DeviceType= session.DeviceType;
                entity.IpAddress= session.IpAddress;
                entity.ExpiryDate= session.ExpiryDate;




                var validationResult = Validator.Validate(entity);
                if (!validationResult.IsValid)
                {
                    foreach (var item in validationResult.Errors)
                    {
                        response.AddError(EErrorCode.SessionSessionUpdateValidationError, item.ErrorMessage);

                    }
                    return response;
                }

                response.Result = Mapper.Map<SessionListDto>(Repository.Update(entity));

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SessionSessionUpdateExceptionError, ex.Message);
            }
            return response;
        }

    }
}
