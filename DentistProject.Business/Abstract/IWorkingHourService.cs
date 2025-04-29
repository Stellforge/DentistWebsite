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
    public interface IWorkingHourService
    {
        public Task<BussinessLayerResult<WorkingHourListDto>> AddOrUpdate(WorkingHourDto workinghour);
        public Task<BussinessLayerResult<WorkingHourListDto>> Delete(long id);
        public Task<BussinessLayerResult<WorkingHourListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<WorkingHourListDto>>> GetAll(LoadMoreFilter<WorkingHourFilter> filter);
        public Task<BussinessLayerResult<int>> Count(WorkingHourFilter filter);

        //public Task<BussinessLayerResult<WorkingHourListDto>> ChangePhoto(WorkingHourDto workinghour);
    }
}

