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
    public interface IServiceService
    {
        public Task<BussinessLayerResult<ServiceListDto>> Add(ServiceDto service);
        public Task<BussinessLayerResult<ServiceListDto>> Delete(long id);
        public Task<BussinessLayerResult<ServiceListDto>> Update(ServiceDto service);
        public Task<BussinessLayerResult<ServiceListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<ServiceListDto>>> GetAll(LoadMoreFilter<ServiceFilter> filter);
        public Task<BussinessLayerResult<int>> Count(ServiceFilter filter);

        public Task<BussinessLayerResult<ServiceListDto>> ChangeLogo(ServiceDto service);
    }
}

