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
    public class InvoiceManager : ServiceBase<InvoiceEntity>, IInvoiceService
    {
        public InvoiceManager(IEntityRepository<InvoiceEntity> repository, IMapper mapper, BaseEntityValidator<InvoiceEntity> validator, IHttpContextAccessor httpContext) : base(repository, mapper, validator, httpContext)
        {
        }

        public async Task<BussinessLayerResult<InvoiceListDto>> Add(InvoiceDto invoice)
        {
            var result = new BussinessLayerResult<InvoiceListDto>();
            try
            {
                var entity = Mapper.Map<InvoiceEntity>(invoice);
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;




                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new Dtos.Error.ErrorDto
                                {
                                    ErrorCode = EErrorCode.InvoiceInvoiceAddValidationError,
                                    Message = x.ErrorMessage
                                }
                    )
                         );
                    return result;
                }

                entity = await Repository.Add(entity);
                result.Result = Mapper.Map<InvoiceListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.InvoiceInvoiceAddExceptionError, ex.Message);

            }
            return result;

        }

        public async Task<BussinessLayerResult<int>> Count(InvoiceFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                  (filter.PatientTreatmentId == null || filter.PatientTreatmentId == x.PatientTreatmentId)
                 && (filter.PaymentType == null || filter.PaymentType == x.PaymentType)
                 && (filter.MaxEndPrice == null || filter.MaxEndPrice >= x.EndPrice)
                 && (filter.MinEndPrice == null || filter.MinEndPrice <= x.EndPrice)
                 //&&( x.IsDeleted == false)
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.InvoiceInvoiceCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<InvoiceListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<InvoiceListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<InvoiceListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.InvoiceInvoiceDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<InvoiceListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<InvoiceListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<InvoiceListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.InvoiceInvoiceGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<InvoiceListDto>>> GetAll(LoadMoreFilter<InvoiceFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<InvoiceListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    //(string.IsNullOrEmpty(filter.Filter.Title) || x.Title.Contains(filter.Filter.Title))
                     (filter.Filter.PatientTreatmentId == null || filter.Filter.PatientTreatmentId == x.PatientTreatmentId)
                 && (filter.Filter.PaymentType == null || filter.Filter.PaymentType == x.PaymentType)
                 && (filter.Filter.MaxEndPrice == null || filter.Filter.MaxEndPrice >= x.EndPrice)
                 && (filter.Filter.MinEndPrice == null || filter.Filter.MinEndPrice <= x.EndPrice)
                    && 
                (x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<InvoiceListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<InvoiceListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<InvoiceListDto>
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
                result.AddError(EErrorCode.InvoiceInvoiceGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<InvoiceListDto>> Update(InvoiceDto invoice)
        {
            var result = new BussinessLayerResult<InvoiceListDto>();
            try
            {
                var entity = await Repository.Get(invoice.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Sale=invoice.Sale;
                entity.Price=invoice.Price;
                entity.EndPrice = invoice.Price - invoice.Sale;
                entity.PaymentType = invoice.PaymentType;

                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.InvoiceInvoiceUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<InvoiceListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.InvoiceInvoiceUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


