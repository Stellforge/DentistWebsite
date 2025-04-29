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
    public interface IAppointmentService
    {
        public Task<BussinessLayerResult<AppointmentListDto>> Add(AppointmentDto appointment,bool force);
        public Task<BussinessLayerResult<AppointmentListDto>> Delete(long id);
        public Task<BussinessLayerResult<AppointmentListDto>> Update(AppointmentDto appointment,bool force);
        public Task<BussinessLayerResult<AppointmentListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AppointmentListDto>>> GetAll(LoadMoreFilter<AppointmentFilter> filter);
        public Task<BussinessLayerResult<int>> Count(AppointmentFilter filter);

        //public Task<BussinessLayerResult<AppointmentListDto>> ChangePhoto(AppointmentDto appointment);
    }
}

