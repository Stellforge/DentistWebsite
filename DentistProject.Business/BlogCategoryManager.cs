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
    public class BlogCategoryManager : ServiceBase<BlogCategoryEntity>, IBlogCategoryService
    {
        public BlogCategoryManager(IEntityRepository<BlogCategoryEntity> repository, IMapper mapper, BaseEntityValidator<BlogCategoryEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<BlogCategoryListDto>> Add(BlogCategoryDto blogcategory)
        {
            var result = new BussinessLayerResult<BlogCategoryListDto>();
            try
            {
                var entity = Mapper.Map<BlogCategoryEntity>(blogcategory);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.BlogCategoryBlogCategoryAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<BlogCategoryListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogCategoryBlogCategoryAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(BlogCategoryFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name))
                 &&(string.IsNullOrEmpty(filter.Explanation) || x.Explanation.Contains(filter.Explanation))
                 //&& (filter.Filter.IsAir == null || filter.Filter.IsAir == x.IsAir)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogCategoryBlogCategoryCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<BlogCategoryListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<BlogCategoryListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<BlogCategoryListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogCategoryBlogCategoryDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<BlogCategoryListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<BlogCategoryListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<BlogCategoryListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogCategoryBlogCategoryGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<BlogCategoryListDto>>> GetAll(LoadMoreFilter<BlogCategoryFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<BlogCategoryListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                     (string.IsNullOrEmpty(filter.Filter.Name) || x.Name.Contains(filter.Filter.Name))
                 && (string.IsNullOrEmpty(filter.Filter.Explanation) || x.Explanation.Contains(filter.Filter.Explanation))
                    //&& (filter.Filter.IsAir == null || filter.Filter.IsAir == x.IsAir)
                &&(x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<BlogCategoryListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<BlogCategoryListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<BlogCategoryListDto>
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
                result.AddError(EErrorCode.BlogCategoryBlogCategoryGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<BlogCategoryListDto>> Update(BlogCategoryDto blogcategory)
        {
            var result = new BussinessLayerResult<BlogCategoryListDto>();
            try
            {
                var entity = await Repository.Get(blogcategory.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Explanation =blogcategory.Explanation;
                entity.Name = blogcategory.Name;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.BlogCategoryBlogCategoryUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<BlogCategoryListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogCategoryBlogCategoryUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


