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
    public interface IAppointmentRequestService
    {
        public Task<BussinessLayerResult<AppointmentRequestListDto>> Add(AppointmentRequestDto appointmentrequest);
        public Task<BussinessLayerResult<AppointmentRequestListDto>> Delete(long id);
        public Task<BussinessLayerResult<AppointmentRequestListDto>> Update(AppointmentRequestDto appointmentrequest);
        public Task<BussinessLayerResult<AppointmentRequestListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AppointmentRequestListDto>>> GetAll(LoadMoreFilter<AppointmentRequestFilter> filter);
        public Task<BussinessLayerResult<int>> Count(AppointmentRequestFilter filter);

        //public Task<BussinessLayerResult<AppointmentRequestListDto>> ChangePhoto(AppointmentRequestDto appointmentrequest);
    }
}

