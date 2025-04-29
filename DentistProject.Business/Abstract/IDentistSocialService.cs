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
    public interface IDentistSocialService
    {
        public Task<BussinessLayerResult<DentistSocialListDto>> Add(DentistSocialDto dentistsocial);
        public Task<BussinessLayerResult<DentistSocialListDto>> Delete(long id);
        public Task<BussinessLayerResult<DentistSocialListDto>> Update(DentistSocialDto dentistsocial);
        public Task<BussinessLayerResult<DentistSocialListDto>> Update(DentistSocialAddDto dentistsocial);
        public Task<BussinessLayerResult<DentistSocialListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<DentistSocialListDto>>> GetAll(LoadMoreFilter<DentistSocialFilter> filter);
        public Task<BussinessLayerResult<int>> Count(DentistSocialFilter filter);

        //public Task<BussinessLayerResult<DentistSocialListDto>> ChangePhoto(DentistSocialDto dentistsocial);
    }
}

