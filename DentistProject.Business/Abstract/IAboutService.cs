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
    public interface IAboutService
    {
        public Task<BussinessLayerResult<AboutListDto>> Add(AboutDto blog);
        public Task<BussinessLayerResult<AboutListDto>> Delete(long id);
        public Task<BussinessLayerResult<AboutListDto>> Update(AboutDto blog);
        public Task<BussinessLayerResult<AboutListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>> GetAll(LoadMoreFilter<AboutFilter> filter);
        public Task<BussinessLayerResult<int>> Count(AboutFilter filter);

        //public Task<BussinessLayerResult<AboutListDto>> ChangePhoto(AboutDto blog);
    }
}
