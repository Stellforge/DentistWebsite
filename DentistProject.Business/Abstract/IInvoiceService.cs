using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.LoadMoreDtos;
using DentistProject.Dtos.Result;
using DentistProject.Filters.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Abstract
{
    public interface IInvoiceService
    {
        public Task<BussinessLayerResult<InvoiceListDto>> Add(InvoiceDto invoice);
        public Task<BussinessLayerResult<InvoiceListDto>> Delete(long id);
        public Task<BussinessLayerResult<InvoiceListDto>> Update(InvoiceDto invoice);
        public Task<BussinessLayerResult<InvoiceListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<InvoiceListDto>>> GetAll(LoadMoreFilter<InvoiceFilter> filter);
        public Task<BussinessLayerResult<int>> Count(InvoiceFilter filter);

        //public Task<BussinessLayerResult<InvoiceListDto>> ChangePhoto(InvoiceDto invoice);
    }
}

