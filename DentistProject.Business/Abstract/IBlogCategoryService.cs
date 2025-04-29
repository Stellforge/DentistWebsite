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
    public interface IBlogCategoryService
    {
        public Task<BussinessLayerResult<BlogCategoryListDto>> Add(BlogCategoryDto blogcategory);
        public Task<BussinessLayerResult<BlogCategoryListDto>> Delete(long id);
        public Task<BussinessLayerResult<BlogCategoryListDto>> Update(BlogCategoryDto blogcategory);
        public Task<BussinessLayerResult<BlogCategoryListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<BlogCategoryListDto>>> GetAll(LoadMoreFilter<BlogCategoryFilter> filter);
        public Task<BussinessLayerResult<int>> Count(BlogCategoryFilter filter);

        //public Task<BussinessLayerResult<BlogCategoryListDto>> ChangePhoto(BlogCategoryDto blogcategory);
    }
}

