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
    public class DentistSocialManager : ServiceBase<DentistSocialEntity>, IDentistSocialService
    {
        public DentistSocialManager(IEntityRepository<DentistSocialEntity> repository, IMapper mapper, BaseEntityValidator<DentistSocialEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<DentistSocialListDto>> Add(DentistSocialDto dentistsocial)
        {
            var result = new BussinessLayerResult<DentistSocialListDto>();
            try
            {
                var entity = Mapper.Map<DentistSocialEntity>(dentistsocial);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.DentistSocialDentistSocialAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<DentistSocialListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistSocialDentistSocialAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(DentistSocialFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.DentistId == null || filter.DentistId == x.DentistId)
                 && (filter.SocialMediaType == null || filter.SocialMediaType == x.SocialMediaType)
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistSocialDentistSocialCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistSocialListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<DentistSocialListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<DentistSocialListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistSocialDentistSocialDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<DentistSocialListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<DentistSocialListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<DentistSocialListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistSocialDentistSocialGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<DentistSocialListDto>>> GetAll(LoadMoreFilter<DentistSocialFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<DentistSocialListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                     (filter.Filter.DentistId == null || filter.Filter.DentistId == x.DentistId)
                    && (filter.Filter.SocialMediaType == null || filter.Filter.SocialMediaType == x.SocialMediaType)
                    && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<DentistSocialListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<DentistSocialListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<DentistSocialListDto>
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
                result.AddError(EErrorCode.DentistSocialDentistSocialGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistSocialListDto>> Update(DentistSocialDto dentistsocial)
        {
            var result = new BussinessLayerResult<DentistSocialListDto>();
            try
            {
                var entity = await Repository.Get(dentistsocial.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Link= dentistsocial.Link;
                entity.Title = dentistsocial.Title;
                entity.SocialMediaType = dentistsocial.SocialMediaType;


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.DentistSocialDentistSocialUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<DentistSocialListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.DentistSocialDentistSocialUpdateExceptionError, ex.Message);

            }
            return result;
        }

        public async Task<BussinessLayerResult<DentistSocialListDto>> Update(DentistSocialAddDto dentistsocial)
        {
            var result= new BussinessLayerResult<DentistSocialListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(dentistsocial.YoutubeLink))
                    {
                        var addResult = await Update(new DentistSocialDto
                        {
                            DentistId = dentistsocial.Id,
                            Link = dentistsocial.YoutubeLink,
                            SocialMediaType = Entities.Enum.ESocialMediaType.Youtube,
                            Title = "Youtube"
                        });
                        if (addResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(addResult.ErrorMessages);
                            return result;
                        }
                    }
                    if (string.IsNullOrEmpty(dentistsocial.FacebookLink))
                    {
                        var addResult = await Update(new DentistSocialDto
                        {
                            DentistId = dentistsocial.Id,
                            Link = dentistsocial.FacebookLink,
                            SocialMediaType = Entities.Enum.ESocialMediaType.Youtube,
                            Title = "Youtube"
                        });
                        if (addResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(addResult.ErrorMessages);
                            return result;
                        }
                    }
                    if (string.IsNullOrEmpty(dentistsocial.InstagramLink))
                    {
                        var addResult = await Update(new DentistSocialDto
                        {
                            DentistId = dentistsocial.Id,
                            Link = dentistsocial.InstagramLink,
                            SocialMediaType = Entities.Enum.ESocialMediaType.Youtube,
                            Title = "Youtube"
                        });
                        if (addResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(addResult.ErrorMessages);
                            return result;
                        }
                    }
                    if (string.IsNullOrEmpty(dentistsocial.XLink))
                    {
                        var addResult = await Update(new DentistSocialDto
                        {
                            DentistId = dentistsocial.Id,
                            Link = dentistsocial.XLink,
                            SocialMediaType = Entities.Enum.ESocialMediaType.Youtube,
                            Title = "Youtube"
                        });
                        if (addResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(addResult.ErrorMessages);
                            return result;
                        }
                    }
                    if (string.IsNullOrEmpty(dentistsocial.WebsiteLink))
                    {
                        var addResult = await Update(new DentistSocialDto
                        {
                            DentistId = dentistsocial.Id,
                            Link = dentistsocial.WebsiteLink,
                            SocialMediaType = Entities.Enum.ESocialMediaType.Youtube,
                            Title = "Youtube"
                        });
                        if (addResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(addResult.ErrorMessages);
                            return result;
                        }
                    }
                    scope.Complete ();

                }
                catch (Exception ex)
                {
                    scope.Dispose ();
                    result.AddError(EErrorCode.DentistSocialDentistSocialAllUpdateExceptionError, ex.Message);

                }
            }
            return result;
        }
    }
}


