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
    public interface IContactService
    {
        public Task<BussinessLayerResult<ContactListDto>> Add(ContactDto contact);
        public Task<BussinessLayerResult<ContactListDto>> Delete(long id);
        public Task<BussinessLayerResult<ContactListDto>> Update(ContactDto contact);
        public Task<BussinessLayerResult<ContactListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<ContactListDto>>> GetAll(LoadMoreFilter<ContactFilter> filter);
        public Task<BussinessLayerResult<int>> Count(ContactFilter filter);

        //public Task<BussinessLayerResult<ContactListDto>> ChangePhoto(ContactDto contact);
    }
}

