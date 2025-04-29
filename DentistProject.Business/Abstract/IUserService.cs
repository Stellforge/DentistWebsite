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
    public interface IUserService
    {
        public Task<BussinessLayerResult<UserListDto>> Add(UserDto user);
        public Task<BussinessLayerResult<UserListDto>> Delete(long id);
        public Task<BussinessLayerResult<UserListDto>> Update(UserDto user);
        public Task<BussinessLayerResult<UserListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<UserListDto>>> GetAll(LoadMoreFilter<UserFilter> filter);
        public Task<BussinessLayerResult<int>> Count(UserFilter filter);

        public Task<BussinessLayerResult<UserListDto>> ChangePhoto(UserDto user);
    }
}

