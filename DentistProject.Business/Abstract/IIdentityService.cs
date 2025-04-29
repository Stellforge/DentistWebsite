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
    public interface IIdentityService
    {
        public Task<BussinessLayerResult<IdentityListDto>> Add(IdentityDto identity);
        public Task<BussinessLayerResult<IdentityListDto>> CheckPassword(IdentityCheckDto identity);
        public Task<BussinessLayerResult<IdentityListDto>> ChangePassword(IdentityDto identity);
        public Task<BussinessLayerResult<IdentityListDto>> ForgatPassword(long userId);


    }
}

