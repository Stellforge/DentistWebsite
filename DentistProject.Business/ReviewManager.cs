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
    public class ReviewManager : ServiceBase<ReviewEntity>, IReviewService
    {
        public ReviewManager(IEntityRepository<ReviewEntity> repository, IMapper mapper, BaseEntityValidator<ReviewEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<ReviewListDto>> Add(ReviewDto review)
        {
            var result = new BussinessLayerResult<ReviewListDto>();
            try
            {
                var entity = Mapper.Map<ReviewEntity>(review);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.ReviewReviewAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<ReviewListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ReviewReviewAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(ReviewFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.Search) || (x.Review + " " + x.Job + " " + x.Name + " " + x.Surname).Contains(filter.Search))
                 && (filter.UserId == null || filter.UserId == x.UserId)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ReviewReviewCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ReviewListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<ReviewListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<ReviewListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ReviewReviewDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<ReviewListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<ReviewListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<ReviewListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ReviewReviewGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<ReviewListDto>>> GetAll(LoadMoreFilter<ReviewFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<ReviewListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.Search) || (x.Review + " " + x.Job + " " + x.Name + " " + x.Surname).Contains(filter.Filter.Search))
                 && (filter.Filter.UserId == null || filter.Filter.UserId == x.UserId)
                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<ReviewListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<ReviewListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<ReviewListDto>
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
                result.AddError(EErrorCode.ReviewReviewGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ReviewListDto>> Update(ReviewDto review)
        {
            var result = new BussinessLayerResult<ReviewListDto>();
            try
            {
                var entity = await Repository.Get(review.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Review = review.Review;
                entity.Surname = review.Surname;
                entity.Job = review.Job;
                entity.Name = review.Name;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.ReviewReviewUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<ReviewListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ReviewReviewUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


