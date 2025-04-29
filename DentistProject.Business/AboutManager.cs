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
    public class AboutManager : ServiceBase<AboutEntity>, IAboutService
    {
        public AboutManager(IEntityRepository<AboutEntity> repository, IMapper mapper, BaseEntityValidator<AboutEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<AboutListDto>> Add(AboutDto about)
        {
            var result = new BussinessLayerResult<AboutListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<AboutEntity>(about);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;




                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.AboutAboutAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    if (entity.IsValid)
                    {
                        var olds = await Repository.GetAll(x => x.IsValid && !x.IsDeleted);
                        foreach (var item in olds)
                        {
                            item.IsValid = false;
                            item.UpdateTime= DateTime.Now;
                            await Repository.Update(item);
                        }
                    }
                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<AboutListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.AboutAboutAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(AboutFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.IsValid == null || filter.IsValid == x.IsValid)

                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AboutAboutCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AboutListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<AboutListDto>();
            try
            {

                var entity = await Repository.Get(id);
                if (entity != null)
                {
                    if (entity.IsValid)
                    {
                        result.AddError(EErrorCode.AboutAboutDeleteDontDeleteValidItemError, "Must be an a valid item");
                        return result;
                    }
                    await Repository.SoftDelete(entity);
                }
            
                result.Result = Mapper.Map<AboutListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AboutAboutDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<AboutListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<AboutListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<AboutListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.AboutAboutGetExceptionError, ex.Message);
            }
            return result;
        }


       


        public async Task<BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>> GetAll(LoadMoreFilter<AboutFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.IsValid == null || filter.Filter.IsValid == x.IsValid)

                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<AboutListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<AboutListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<AboutListDto>
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
                result.AddError(EErrorCode.AboutAboutGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<AboutListDto>> Update(AboutDto about)
        {
            var result = new BussinessLayerResult<AboutListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(about.Id);
                    entity.IsDeleted = false;

                    entity.UpdateTime = DateTime.Now;


                    entity.MissionStatement = about.MissionStatement;
                    entity.OurWork = about.OurWork;
                    entity.Target = about.Target;
                    entity.Explanation = about.Explanation;


                    if(entity.IsValid == true &&about.IsValid==false && await Repository.CountAsync(x=>x.IsValid&&x.Id!=entity.Id)==0)
                    {
                        scope.Dispose();    
                        result.AddError(EErrorCode.AboutAboutUpdateMustValidItemError, "Must be one valid item.");
                        return result;
                    }


                    if (entity.IsValid == false && about.IsValid == true )
                    {
                        var olds = await Repository.GetAll(x => x.IsValid && x.Id != entity.Id);
                        foreach (var item in olds)
                        {
                            item.IsValid = false;
                            item.UpdateTime = DateTime.Now;
                            await Repository.Update(item);
                        }
                    }

                    entity.IsValid = about.IsValid;

                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new ErrorDto
                                    {
                                        ErrorCode = EErrorCode.AboutAboutUpdateValidationError,
                                        Message = x.ErrorMessage
                                    }
                                 )
                             );
                        return result;
                    }

                    entity = await Repository.Update(entity);
                    result.Result = Mapper.Map<AboutListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    result.AddError(EErrorCode.AboutAboutUpdateExceptionError, ex.Message);

                }
            }
            return result;
        }
    }
}
