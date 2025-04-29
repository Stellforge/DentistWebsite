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
    public interface IPatientTreatmentServicesService
    {
        public Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Add(PatientTreatmentServicesDto patienttreatmentservices);
        public Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Update(PatientTreatmentServicesDto patienttreatmentservices);
        public Task<BussinessLayerResult<PatientTreatmentServicesListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientTreatmentServicesListDto>>> GetAll(LoadMoreFilter<PatientTreatmentServicesFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientTreatmentServicesFilter filter);

        //public Task<BussinessLayerResult<PatientTreatmentServicesListDto>> ChangePhoto(PatientTreatmentServicesDto patienttreatmentservices);
    }
}

