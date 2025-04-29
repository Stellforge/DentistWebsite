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
using DentistProject.Entities.Enum;
using DentistProject.Core.ExtensionMethods;

namespace DentistProject.Business
{
    public class SystemSettingManager : ServiceBase<SystemSettingEntity>, ISystemSettingService
    {
        private readonly IMediaService _mediaService;

        public SystemSettingManager(IEntityRepository<SystemSettingEntity> repository, IMapper mapper, BaseEntityValidator<SystemSettingEntity> validator, IHttpContextAccessor httpContext, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> Add(SystemSettingDto systemsetting)
        {
            var result = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity = Mapper.Map<SystemSettingEntity>(systemsetting);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.SystemSettingSystemSettingAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<SystemSettingListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.SystemSettingSystemSettingAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(SystemSettingFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  //(string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title))
                  (filter.Key == null || filter.Key == x.Key)
                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.SystemSettingSystemSettingCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<SystemSettingListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.SystemSettingSystemSettingDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<SystemSettingListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.SystemSettingSystemSettingGetExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> Get(ESettingKey key)
        {
            var response = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity = Repository.Get(x => x.Key == key);
                var dto = Mapper.Map<SystemSettingListDto>(entity);



                response.Result = dto;

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SystemSettingSystemSettingGetExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<SystemSettingListDto>>> GetAll(LoadMoreFilter<SystemSettingFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<SystemSettingListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                   (filter.Filter.Key == null || filter.Filter.Key == x.Key)

                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<SystemSettingListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<SystemSettingListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<SystemSettingListDto>
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
                result.AddError(EErrorCode.SystemSettingSystemSettingGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> Update(SystemSettingDto systemsetting)
        {
            var result = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity = await Repository.Get(systemsetting.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Value = systemsetting.Value;



                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.SystemSettingSystemSettingUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<SystemSettingListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.SystemSettingSystemSettingUpdateExceptionError, ex.Message);

            }
            return result;
        }



        public async Task<BussinessLayerResult<SmtpValues>> GetSmtp()
        {
            var response = new BussinessLayerResult<SmtpValues>();
            try
            {
                var smtp = new SmtpValues
                {
                    SmtpDisplayAddress = (await Repository.Get(x => x.Key == ESettingKey.SmtpDisplayAddress)).Value,
                    SmtpDisplayName = (await Repository.Get(x => x.Key == ESettingKey.SmtpDisplayName)).Value,
                    SmtpEnableSsl = (bool.Parse((await Repository.Get(x => x.Key == ESettingKey.SmtpEnableSsl)).Value)),
                    SmtpPassword = (await Repository.Get(x => x.Key == ESettingKey.SmtpPassword)).Value,
                    SmtpPort = (Convert.ToInt32((await Repository.Get(x => x.Key == ESettingKey.SmtpPort)).Value)),
                    SmtpServer = (await Repository.Get(x => x.Key == ESettingKey.SmtpServer)).Value,
                    SmtpUsername = (await Repository.Get(x => x.Key == ESettingKey.SmtpUsername)).Value

                };


                response.Result = smtp;

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SystemSettingSystemSettingGetExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SystemSettingListDto>> GetLogo()
        {
            var response = new BussinessLayerResult<SystemSettingListDto>();
            try
            {
                var entity =await Repository.Get(x => x.Key == ESettingKey.Logo);
                var dto = Mapper.Map<SystemSettingListDto>(entity);

                response.Result = dto;

            }
            catch (Exception ex)
            {
                response.AddError(EErrorCode.SystemSettingSystemSettingGetExceptionError, ex.Message);
            }
            return response;
        }
        public async Task<BussinessLayerResult<bool>> ChangeLogo(LogoDto logo)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = await Repository.Get(logo.Id);
                var mediaResult = (string.IsNullOrEmpty(entity?.Value))
                    ? await _mediaService.Add(new MediaDto { File = logo.File })
                    : await _mediaService.Update(new MediaDto { File = logo.File, Id = Convert.ToInt64(entity.Value) });
                if (mediaResult.Status == EResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                    return response;
                }

                entity.Value = mediaResult.Result.Id.ToString();
                response.Result = true;
                Repository.Update(entity);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(EErrorCode.SystemSettingSystemSettingChangeLogoExceptionError, ex.Message);

            }
            return response;
        }





    }
}


