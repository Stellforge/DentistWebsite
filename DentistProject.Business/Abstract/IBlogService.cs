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
    public interface IBlogService
    {
        public Task<BussinessLayerResult<BlogListDto>> Add(BlogDto blog);
        public Task<BussinessLayerResult<BlogListDto>> Delete(long id);
        public Task<BussinessLayerResult<BlogListDto>> Update(BlogDto blog);
        public Task<BussinessLayerResult<BlogListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>> GetAll(LoadMoreFilter<BlogFilter> filter);
        public Task<BussinessLayerResult<int>> Count(BlogFilter filter);

        //public Task<BussinessLayerResult<BlogListDto>> ChangePhoto(BlogDto blog);
    }
}

