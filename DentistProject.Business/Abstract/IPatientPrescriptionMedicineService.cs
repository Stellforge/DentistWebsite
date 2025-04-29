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
    public interface IPatientPrescriptionMedicineService
    {
        public Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Add(PatientPrescriptionMedicineDto patientprescriptionmedicine);
        public Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Update(PatientPrescriptionMedicineDto patientprescriptionmedicine);
        public Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientPrescriptionMedicineListDto>>> GetAll(LoadMoreFilter<PatientPrescriptionMedicineFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientPrescriptionMedicineFilter filter);

        //public Task<BussinessLayerResult<PatientPrescriptionMedicineListDto>> ChangePhoto(PatientPrescriptionMedicineDto patientprescriptionmedicine);
    }
}

