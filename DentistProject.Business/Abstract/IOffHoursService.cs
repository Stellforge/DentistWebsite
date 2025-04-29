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
    public interface IOffHoursService
    {
        public Task<BussinessLayerResult<OffHoursListDto>> Add(OffHoursDto offhours,bool force);
        public Task<BussinessLayerResult<OffHoursListDto>> Delete(long id);
        public Task<BussinessLayerResult<OffHoursListDto>> Update(OffHoursDto offhours);
        public Task<BussinessLayerResult<OffHoursListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<OffHoursListDto>>> GetAll(LoadMoreFilter<OffHoursFilter> filter);
        public Task<BussinessLayerResult<int>> Count(OffHoursFilter filter);

        //public Task<BussinessLayerResult<OffHoursListDto>> ChangePhoto(OffHoursDto offhours);
    }
}

