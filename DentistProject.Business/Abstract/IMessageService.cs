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
    public interface IMessageService
    {
        public Task<BussinessLayerResult<MessageListDto>> Add(MessageDto message);
        public Task<BussinessLayerResult<MessageListDto>> Delete(long id);
        public Task<BussinessLayerResult<MessageListDto>> Update(MessageDto message);
        public Task<BussinessLayerResult<MessageListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<MessageListDto>>> GetAll(LoadMoreFilter<MessageFilter> filter);
        public Task<BussinessLayerResult<int>> Count(MessageFilter filter);

        //public Task<BussinessLayerResult<MessageListDto>> ChangePhoto(MessageDto message);
    }
}

