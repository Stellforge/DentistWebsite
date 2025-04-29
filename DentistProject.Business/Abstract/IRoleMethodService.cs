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
    public interface IRoleMethodService
    {
        public Task<BussinessLayerResult<List<RoleMethodListDto>>> AddOrUpdate(RoleMethodDto rolemethod);      
        public Task<BussinessLayerResult<RoleMethodListDto>> Delete(long id);
        public Task<BussinessLayerResult<RoleMethodListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>> GetAll(LoadMoreFilter<RoleMethodFilter> filter);
        public Task<BussinessLayerResult<int>> Count(RoleMethodFilter filter);
        public Task<BussinessLayerResult<RoleMethodListDto>> UpdateAll(RoleMethodAllUpdateDto roleMethod);

        //public Task<BussinessLayerResult<RoleMethodListDto>> ChangePhoto(RoleMethodDto rolemethod);
    }
}

