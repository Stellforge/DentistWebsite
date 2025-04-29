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
    public class RoleMethodManager : ServiceBase<RoleMethodEntity>, IRoleMethodService
    {
        public RoleMethodManager(IEntityRepository<RoleMethodEntity> repository, IMapper mapper, BaseEntityValidator<RoleMethodEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<List<RoleMethodListDto>>> AddOrUpdate(RoleMethodDto rolemethod)
        {
            var result = new BussinessLayerResult<List<RoleMethodListDto>>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var oldEntities = await Repository.GetAll(x => x.IsDeleted == false && x.Role == rolemethod.Role);
                    foreach (var item in oldEntities)
                    {
                        item.UpdateTime = DateTime.Now;
                        Repository.SoftDelete(item);
                    }

                    result.Result = new List<RoleMethodListDto>();
                    foreach (var method in rolemethod.Method)
                    {
                        var entity = new RoleMethodEntity
                        {
                            CreateTime = DateTime.Now,
                            IsDeleted = false,
                            Role = rolemethod.Role,
                            Method = method

                        };

                        var validationResult = await Validator.ValidateAsync(entity);
                        if (!validationResult.IsValid)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(
                                    validationResult.Errors.Select(x =>
                                        new Dtos.Error.ErrorDto
                                        {
                                            ErrorCode = EErrorCode.RoleMethodRoleMethodAddValidationError,
                                            Message = x.ErrorMessage
                                        }
                            )
                                 );
                            return result;
                        }

                        entity = await Repository.Add(entity);
                        result.Result.Add(Mapper.Map<RoleMethodListDto>(entity));
                    }
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.RoleMethodRoleMethodAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(RoleMethodFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  (filter.Role == null || filter.Role == x.Role)
                 && (filter.Method == null || filter.Method == x.Method)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.RoleMethodRoleMethodCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<RoleMethodListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<RoleMethodListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<RoleMethodListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.RoleMethodRoleMethodDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<RoleMethodListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<RoleMethodListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<RoleMethodListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.RoleMethodRoleMethodGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>> GetAll(LoadMoreFilter<RoleMethodFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                  (filter.Filter.Role == null || filter.Filter.Role == x.Role)
                 && (filter.Filter.Method == null || filter.Filter.Method == x.Method)
                 && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<RoleMethodListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<RoleMethodListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<RoleMethodListDto>
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
                result.AddError(EErrorCode.RoleMethodRoleMethodGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<RoleMethodListDto>> UpdateAll(RoleMethodAllUpdateDto roleMethod)
        {
            var response = new BussinessLayerResult<RoleMethodListDto>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entities = await Repository.GetAll(x => x.Role == roleMethod.Role);
                    foreach (var item in entities)
                    {
                       await Repository.SoftDelete(item);
                    }

                    //Yeni kayýtlar
                    var list = roleMethod.Methods.Select(x => new RoleMethodEntity
                    {
                        IsDeleted = false,
                        Method = x,
                        Role = roleMethod.Role,
                        CreateTime = DateTime.Now,
                        

                    }).ToList();

                    foreach (var item in list)
                    {
                       await Repository.Add(item);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    response.AddError(EErrorCode.RoleMethodRoleMethodUpdateExceptionError, ex.Message);
                }
            }
            return response;
        }


    }
}


