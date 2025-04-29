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
using static System.Formats.Asn1.AsnWriter;

namespace DentistProject.Business
{
    public class ContactManager : ServiceBase<ContactEntity>, IContactService
    {
        public ContactManager(IEntityRepository<ContactEntity> repository, IMapper mapper, BaseEntityValidator<ContactEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<ContactListDto>> Add(ContactDto contact)
        {
            var result = new BussinessLayerResult<ContactListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<ContactEntity>(contact);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    entity.FacebookLink = entity.FacebookLink ?? "";
                    entity.Latitude = entity.Latitude ?? "";
                    entity.Longitude = entity.Longitude ?? "";


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.ContactContactAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }


                    if (entity.Validity)
                    {
                        var olds = await Repository.GetAll(x => x.Validity && !x.IsDeleted);
                        foreach (var item in olds)
                        {
                            item.Validity = false;
                            item.UpdateTime = DateTime.Now;
                            await Repository.Update(item);
                        }
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<ContactListDto>(entity);
                    scope.Complete();

                }
                
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.ContactContactAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(ContactFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                  //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.Validity == null || filter.Validity == x.Validity)

                 && (x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ContactContactCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ContactListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<ContactListDto>();
            try
            {
                var entity = await Repository.Get(id);
                if (entity != null)
                {
                    if (entity.Validity)
                    {
                        result.AddError(EErrorCode.AboutAboutDeleteDontDeleteValidItemError, "Must be an a valid item");
                        return result;
                    }
                    await Repository.SoftDelete(entity);
                }
                result.Result = Mapper.Map<ContactListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ContactContactDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<ContactListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<ContactListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<ContactListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.ContactContactGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<ContactListDto>>> GetAll(LoadMoreFilter<ContactFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<ContactListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.Validity == null || filter.Filter.Validity == x.Validity)
                    &&
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<ContactListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<ContactListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<ContactListDto>
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
                result.AddError(EErrorCode.ContactContactGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<ContactListDto>> Update(ContactDto contact)
        {
            var result = new BussinessLayerResult<ContactListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(contact.Id);
                    entity.IsDeleted = false;

                    entity.UpdateTime = DateTime.Now;


                    entity.Longitude = contact.Longitude??"";
                    entity.Latitude = contact.Latitude??"";
                    entity.Name = contact.Name ?? "";
                    entity.Adress = contact.Adress ?? "";
                    entity.Phone1 = contact.Phone1 ?? "";
                    entity.Phone2 = contact.Phone2 ?? "";
                    entity.FacebookLink = contact.FacebookLink ?? "";
                    entity.GoogleMapLink = contact.GoogleMapLink ?? "";
                    entity.YoutubeLink = contact.YoutubeLink ?? "";
                    entity.Email = contact.Email ?? "";
                    entity.InstagramLink = contact.InstagramLink ?? "";
                    entity.XLink = contact.XLink ?? "";


                    if (entity.Validity == true && contact.Validity == false && await Repository.CountAsync(x => x.Validity && x.Id != entity.Id) == 0)
                    {
                        scope.Dispose();
                        result.AddError(EErrorCode.AboutAboutUpdateMustValidItemError, "Must be one valid item.");
                        return result;
                    }


                    if (entity.Validity == false && contact.Validity == true)
                    {
                        var olds = await Repository.GetAll(x => x.Validity && x.Id != entity.Id);
                        foreach (var item in olds)
                        {
                            item.Validity = false;
                            item.UpdateTime = DateTime.Now;
                            await Repository.Update(item);
                        }
                    }

                    entity.Validity = contact.Validity;


                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new ErrorDto
                                    {
                                        ErrorCode = EErrorCode.ContactContactUpdateValidationError,
                                        Message = x.ErrorMessage
                                    }
                                 )
                             );
                        return result;
                    }

                    entity = await Repository.Update(entity);
                    result.Result = Mapper.Map<ContactListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.ContactContactUpdateExceptionError, ex.Message);

                }
            }
            return result;
        }
    }
}


