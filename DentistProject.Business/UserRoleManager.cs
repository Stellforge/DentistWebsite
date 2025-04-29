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
    public class UserRoleManager : ServiceBase<UserRoleEntity>, IUserRoleService
    {
        public UserRoleManager(IEntityRepository<UserRoleEntity> repository, IMapper mapper, BaseEntityValidator<UserRoleEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<UserRoleListDto>> Add(UserRoleDto userrole)
        {
            var result = new BussinessLayerResult<UserRoleListDto>();
            try
            {
                var entity = Mapper.Map<UserRoleEntity>(userrole);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.UserRoleUserRoleAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<UserRoleListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserRoleUserRoleAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(UserRoleFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.UserId == null || filter.UserId == x.UserId)
                 && (filter.Role == null || filter.Role == x.Role)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserRoleUserRoleCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<UserRoleListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<UserRoleListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<UserRoleListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserRoleUserRoleDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<UserRoleListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<UserRoleListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<UserRoleListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserRoleUserRoleGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>> GetAll(LoadMoreFilter<UserRoleFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                (filter.Filter.UserId == null || filter.Filter.UserId == x.UserId)
                 && (filter.Filter.Role == null || filter.Filter.Role == x.Role)
                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<UserRoleListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<UserRoleListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<UserRoleListDto>
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
                result.AddError(EErrorCode.UserRoleUserRoleGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<UserRoleListDto>> Update(UserRoleDto userrole)
        {
            var result = new BussinessLayerResult<UserRoleListDto>();
            try
            {
                var entity = await Repository.Get(userrole.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Role = userrole.Role;

                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.UserRoleUserRoleUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<UserRoleListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.UserRoleUserRoleUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


