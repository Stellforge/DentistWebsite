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
    public interface IUserRoleService
    {
        public Task<BussinessLayerResult<UserRoleListDto>> Add(UserRoleDto userrole);
        public Task<BussinessLayerResult<UserRoleListDto>> Delete(long id);
        public Task<BussinessLayerResult<UserRoleListDto>> Update(UserRoleDto userrole);
        public Task<BussinessLayerResult<UserRoleListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>> GetAll(LoadMoreFilter<UserRoleFilter> filter);
        public Task<BussinessLayerResult<int>> Count(UserRoleFilter filter);

        //public Task<BussinessLayerResult<UserRoleListDto>> ChangePhoto(UserRoleDto userrole);
    }
}

