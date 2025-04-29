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
    public interface IPatientTreatmentService
    {
        public Task<BussinessLayerResult<PatientTreatmentListDto>> Add(PatientTreatmentDto patienttreatment);
        public Task<BussinessLayerResult<PatientTreatmentListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientTreatmentListDto>> Update(PatientTreatmentDto patienttreatment);
        public Task<BussinessLayerResult<PatientTreatmentListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentListDto>>> GetAll(LoadMoreFilter<PatientTreatmentFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientTreatmentFilter filter);

        //public Task<BussinessLayerResult<PatientTreatmentListDto>> ChangePhoto(PatientTreatmentDto patienttreatment);
    }
}

