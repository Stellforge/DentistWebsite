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
    public interface IMediaService
    {
        public Task<BussinessLayerResult<MediaListDto>> Add(MediaDto media);
        public Task<BussinessLayerResult<bool>> Delete(long id);
        public Task<BussinessLayerResult<MediaListDto>> Update(MediaDto media);
        public Task<BussinessLayerResult<MediaListDto>> Get(long id);
    }
}

