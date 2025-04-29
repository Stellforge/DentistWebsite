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
    public interface IPatientService
    {
        public Task<BussinessLayerResult<PatientListDto>> Add(PatientDto patient);
        public Task<BussinessLayerResult<PatientListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientListDto>> Update(PatientDto patient);
        public Task<BussinessLayerResult<PatientListDto>> Get(long id);
        public Task<BussinessLayerResult<PatientListDto>> GetByUserId(long userId);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientListDto>>> GetAll(LoadMoreFilter<PatientFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientFilter filter);

        //public Task<BussinessLayerResult<PatientListDto>> ChangePhoto(PatientDto patient);
    }
}

