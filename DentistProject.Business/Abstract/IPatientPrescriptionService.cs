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
    public interface IPatientPrescriptionService
    {
        public Task<BussinessLayerResult<PatientPrescriptionListDto>> Add(PatientPrescriptionDto patientprescription);
        public Task<BussinessLayerResult<PatientPrescriptionListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientPrescriptionListDto>> Update(PatientPrescriptionDto patientprescription);
        public Task<BussinessLayerResult<PatientPrescriptionListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionListDto>>> GetAll(LoadMoreFilter<PatientPrescriptionFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientPrescriptionFilter filter);

        //public Task<BussinessLayerResult<PatientPrescriptionListDto>> ChangePhoto(PatientPrescriptionDto patientprescription);
    }
}

