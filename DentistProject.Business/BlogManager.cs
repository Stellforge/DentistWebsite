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
    public class BlogManager : ServiceBase<BlogEntity>, IBlogService
    {

        private readonly IMediaService _mediaService;

        public BlogManager(IEntityRepository<BlogEntity> repository, IMapper mapper, BaseEntityValidator<BlogEntity> validator, IHttpContextAccessor httpContext, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<BlogListDto>> Add(BlogDto blog)
        {
            var result = new BussinessLayerResult<BlogListDto>();
            try
            {
                var entity = Mapper.Map<BlogEntity>(blog);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;
                entity.PublicationDate = (entity.OnAir) ? DateTime.Now : null;

                var mediaResult = await _mediaService.Add(new MediaDto { File = blog.Photo });
                if(mediaResult.Status==EResultStatus.Error)
                {
                    result.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return result;  
                }

                entity.PhotoId = mediaResult.Result.Id;



                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.BlogBlogAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<BlogListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogBlogAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(BlogFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Search) ||
                     (x.Title + " " + x.Abstract + " " +
                     x.Content + " " + x.Keyword + " " +
                     x.PublicationDate.ToString() + " " +
                     x.User.Name + " "+
                     x.User.Surname + " "
                     ).Contains(filter.Search))
                 && (filter.UserId == null || filter.UserId == x.UserId)
                 && (filter.CategoryId == null || filter.CategoryId == x.CategoryId)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogBlogCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<BlogListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<BlogListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<BlogListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogBlogDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<BlogListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<BlogListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<BlogListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogBlogGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>> GetAll(LoadMoreFilter<BlogFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.Search) ||
                     (x.Title + " " + x.Abstract + " " +
                     x.Content + " " + x.Keyword + " " +
                     x.PublicationDate.ToString() + " " +
                     x.User.Name + " " +
                     x.User.Surname + " "
                     ).Contains(filter.Filter.Search))
                 && (filter.Filter.UserId == null || filter.Filter.UserId == x.UserId)
                 && (filter.Filter.CategoryId == null || filter.Filter.CategoryId == x.CategoryId)
                && (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<BlogListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<BlogListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<BlogListDto>
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
                result.AddError(EErrorCode.BlogBlogGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<BlogListDto>> Update(BlogDto blog)
        {
            var result = new BussinessLayerResult<BlogListDto>();
            try
            {
                var entity = await Repository.Get(blog.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;



                entity.CategoryId = blog.CategoryId;
                entity.Abstract = blog.Abstract;
                entity.Content = blog.Content;
                entity.Keyword = blog.Keyword;
                entity.Title = blog.Title;
                //entity.UserId = blog.UserId;

                if (entity.OnAir == false && blog.OnAir == true)
                {
                    entity.PublicationDate = DateTime.Now;
                    entity.OnAir = blog.OnAir;
                }


                var mediaResult = (entity.PhotoId == null)
                    ? await _mediaService.Add(new MediaDto { File = blog.Photo })
                    : await _mediaService.Update(new MediaDto { File = blog.Photo, Id = entity.PhotoId });
                    
                if (mediaResult.Status == EResultStatus.Error)
                {
                    result.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return result;
                }

                entity.PhotoId = mediaResult.Result.Id;







                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.BlogBlogUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<BlogListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.BlogBlogUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


