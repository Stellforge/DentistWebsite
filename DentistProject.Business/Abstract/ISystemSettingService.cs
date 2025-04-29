using DentistProject.Core.ExtensionMethods;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.LoadMoreDtos;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Enum;
using DentistProject.Filters.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Abstract
{
    public interface ISystemSettingService
    {
        public Task<BussinessLayerResult<SystemSettingListDto>> Add(SystemSettingDto systemsetting);
        public Task<BussinessLayerResult<SystemSettingListDto>> Delete(long id);
        public Task<BussinessLayerResult<SystemSettingListDto>> Update(SystemSettingDto systemsetting);
        public Task<BussinessLayerResult<SystemSettingListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<SystemSettingListDto>>> GetAll(LoadMoreFilter<SystemSettingFilter> filter);
        public Task<BussinessLayerResult<int>> Count(SystemSettingFilter filter);

        public Task<BussinessLayerResult<SystemSettingListDto>> Get(ESettingKey key);

        public Task<BussinessLayerResult<SmtpValues>> GetSmtp();
        public Task<BussinessLayerResult<SystemSettingListDto>> GetLogo();
        public Task<BussinessLayerResult<bool>> ChangeLogo(LogoDto logo);



    }
}

