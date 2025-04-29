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
using FluentValidation;

namespace DentistProject.Business
{
    public class MediaManager : ServiceBase<MediaEntity>, IMediaService
    {
        string path = "C:/DentistProject.Medias";
        public MediaManager(IEntityRepository<MediaEntity> repository, IMapper mapper, BaseEntityValidator<MediaEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        public async Task<BussinessLayerResult<MediaListDto>> Add(MediaDto media)
        {
            var response = new BussinessLayerResult<MediaListDto>();
            try
            {

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(media.File.FileName);
                var filePath = Path.Combine(path, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await media.File.CopyToAsync(fs);
                }

                var entity = new MediaEntity
                {
                    FileName = media.File.FileName,
                    FileType = media.File.ContentType,
                    FileUrl = filePath,
                    CreateTime = DateTime.Now,
                    IsDeleted = false
                };
                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    await Repository.Add(entity);
                    response.Result = new MediaListDto { Id = entity.Id };
                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(EErrorCode.MediaMediaAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(EErrorCode.MediaMediaAddExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Delete(long id)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = await Repository.Get(id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    await  Repository.Update(entity);
                    response.Result = true;

                }
                else
                {
                    response.Result = false;
                    response.AddError(EErrorCode.MediaMediaDeleteItemNotFoundError, "");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(EErrorCode.MediaMediaDeleteExceptionError,
                                  ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<MediaListDto>> Update(MediaDto media)
        {
            var response = new BussinessLayerResult<MediaListDto>();
            try
            {
                var entity = await Repository.Get(media.Id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(EErrorCode.MediaMediaUpdateItemNotFoundError, "");
                    return response;
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                using (var fs = new FileStream(entity.FileUrl, FileMode.Truncate))
                {
                    await media.File.CopyToAsync(fs);
                }
                entity.FileType = media.File.ContentType;
                entity.FileName = media.File.FileName;

                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Update(entity);
                    response.Result = new MediaListDto { Id = entity.Id };
                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(EErrorCode.MediaMediaUpdateValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(EErrorCode.MediaMediaUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<MediaListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<MediaListDto>();
            try
            {
                var entity = await Repository.Get(id);
                if (entity == null)
                {
                    response.AddError(EErrorCode.MediaMediaGetItemNotFoundError, "");

                }
                else
                {

                    var dto = new MediaListDto
                    {
                        Id = id,
                        FileName = entity.FileName,
                        FileType = entity.FileType,
                        File = await File.ReadAllBytesAsync(entity.FileUrl)
                    };
                    response.Result = dto;

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(EErrorCode.MediaMediaGetExceptionError, ex.Message);
            }
            return response;

        }


    }
}


