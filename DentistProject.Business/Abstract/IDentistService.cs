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
    public interface IDentistService
    {
        public Task<BussinessLayerResult<DentistListDto>> Add(DentistDto dentist);
        public Task<BussinessLayerResult<DentistListDto>> Delete(long id);
        public Task<BussinessLayerResult<DentistListDto>> Update(DentistDto dentist);
        public Task<BussinessLayerResult<DentistListDto>> Get(long id);
        public Task<BussinessLayerResult<DentistListDto>> GetByUserId(long userId);
        public Task<BussinessLayerResult<GenericLoadMoreDto<DentistListDto>>> GetAll(LoadMoreFilter<DentistFilter> filter);
        public Task<BussinessLayerResult<int>> Count(DentistFilter filter);

        public Task<BussinessLayerResult<DentistListDto>> ChangePhoto(DentistDto dentist);
    }
}

