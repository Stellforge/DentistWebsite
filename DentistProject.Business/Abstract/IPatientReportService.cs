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
    public interface IPatientReportService
    {
        public Task<BussinessLayerResult<PatientReportListDto>> Add(PatientReportDto patientreport);
        public Task<BussinessLayerResult<PatientReportListDto>> Delete(long id);
        public Task<BussinessLayerResult<PatientReportListDto>> Update(PatientReportDto patientreport);
        public Task<BussinessLayerResult<PatientReportListDto>> Get(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PatientReportListDto>>> GetAll(LoadMoreFilter<PatientReportFilter> filter);
        public Task<BussinessLayerResult<int>> Count(PatientReportFilter filter);

        public Task<BussinessLayerResult<PatientReportListDto>> ChangeFile(PatientReportDto patientreport);
    }
}

