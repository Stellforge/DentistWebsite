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
    public interface IReviewService
    {
        public Task<BussinessLayerResult<ReviewListDto>> Add(ReviewDto review);
        public Task<BussinessLayerResult<ReviewListDto>> Delete(long id);
        public Task<BussinessLayerResult<ReviewListDto>> Update(ReviewDto review);
        public Task<BussinessLayerResult<ReviewListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<ReviewListDto>>> GetAll(LoadMoreFilter<ReviewFilter> filter);
        public Task<BussinessLayerResult<int>> Count(ReviewFilter filter);

        //public Task<BussinessLayerResult<ReviewListDto>> ChangePhoto(ReviewDto review);
    }
}

