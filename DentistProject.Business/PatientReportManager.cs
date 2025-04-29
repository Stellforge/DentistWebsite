using AutoMapper;
using DentistProject.Business.Abstract;
using DentistProject.Core.DataAccess;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.LoadMoreDtos;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Abstract;
using DentistProject.Entities;
using DentistProject.Filters.Filter;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.Error;
using System.Transactions;

namespace DentistProject.Business
{
    public class PatientReportManager : ServiceBase<PatientReportEntity>, IPatientReportService
    {
        private readonly IMediaService _mediaService;
        public PatientReportManager(IEntityRepository<PatientReportEntity> repository, IMapper mapper, BaseEntityValidator<PatientReportEntity> validator, IHttpContextAccessor httpContext, IMediaService mediaService) : base(repository, mapper, validator, httpContext)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<PatientReportListDto>> Add(PatientReportDto patientreport)
        {
            var result = new BussinessLayerResult<PatientReportListDto>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Mapper.Map<PatientReportEntity>(patientreport);
                    entity.IsDeleted = false;
                    entity.CreateTime = DateTime.Now;

                    var fileResult = await _mediaService.Add(new MediaDto { File = patientreport.File });
                    if (fileResult.Status == EResultStatus.Error)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(fileResult.ErrorMessages);
                        return result;
                    }

                    entity.FileId = fileResult.Result.Id;

                    var validationResult = await Validator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        scope.Dispose();
                        result.ErrorMessages.AddRange(
                                validationResult.Errors.Select(x =>
                                    new Dtos.Error.ErrorDto
                                    {
                                        ErrorCode = EErrorCode.PatientReportPatientReportAddValidationError,
                                        Message = x.ErrorMessage
                                    }
                        )
                             );
                        return result;
                    }

                    entity = await Repository.Add(entity);
                    result.Result = Mapper.Map<PatientReportListDto>(entity);


                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose( );
                    result.AddError(EErrorCode.PatientReportPatientReportAddExceptionError, ex.Message);

                }
            }
            return result;

        }

        public async Task<BussinessLayerResult<PatientReportListDto>> ChangeFile(PatientReportDto patientReport)
        {
            var result = new BussinessLayerResult<PatientReportListDto>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = await Repository.Get(patientReport.Id);
                    if (entity != null)
                    {
                        var mediaResult = (entity.FileId != null)
                             ? await _mediaService.Update(new MediaDto { File = patientReport.File, Id = entity.FileId })
                             : await _mediaService.Add(new MediaDto { File = patientReport.File });
                        if (mediaResult.Status == EResultStatus.Error)
                        {
                            scope.Dispose();
                            result.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                            return result;
                        }

                    }
                    result.Result = Mapper.Map<PatientReportListDto>(entity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result.AddError(EErrorCode.PatientReportPatientReportDeleteExceptionError, ex.Message);
                }
            }
            return result;
        }

        public async Task<BussinessLayerResult<int>> Count(PatientReportFilter filter)
        {
            var result = new BussinessLayerResult<int>();
            try
            {
                result.Result = (filter != null) ?
                     await Repository.CountAsync(x =>
                 (string.IsNullOrEmpty(filter.ReportType) || x.Title.Contains(filter.ReportType))
                 &&(string.IsNullOrEmpty(filter.Search) || (x.Title+" "+x.Explanation+" "+x.ReportType).Contains(filter.Search))
                // && (filter.TreatmentId == null || filter.TreatmentId == x.TreatmentId)
                 && (filter.PatientId == null || filter.PatientId == x.PatientId)
             
                 &&(x.IsDeleted == false)
                 ) : await Repository.CountAsync(x => x.IsDeleted == false);


            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientReportPatientReportCountExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientReportListDto>> Delete(long id)
        {
            var result = new BussinessLayerResult<PatientReportListDto>();
            try
            {
                var entity = await Repository.SoftDelete(id);
                result.Result = Mapper.Map<PatientReportListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientReportPatientReportDeleteExceptionError, ex.Message);
            }
            return result;



        }

        public async Task<BussinessLayerResult<PatientReportListDto>> Get(long id)
        {
            var result = new BussinessLayerResult<PatientReportListDto>();
            try
            {
                var entity = await Repository.Get(id);
                result.Result = Mapper.Map<PatientReportListDto>(entity);
            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientReportPatientReportGetExceptionError, ex.Message);
            }
            return result;
        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<PatientReportListDto>>> GetAll(LoadMoreFilter<PatientReportFilter> filter)
        {
            var result = new BussinessLayerResult<GenericLoadMoreDto<PatientReportListDto>>();
            try
            {
                var entities = (filter.Filter != null) ?
                    await Repository.GetAll(x =>
                    (string.IsNullOrEmpty(filter.Filter.ReportType) || x.Title.Contains(filter.Filter.ReportType))
                 && (string.IsNullOrEmpty(filter.Filter.Search) || (x.Title + " " + x.Explanation + " " + x.ReportType).Contains(filter.Filter.Search))
                // && (filter.Filter.TreatmentId == null || filter.Filter.TreatmentId == x.TreatmentId)
                 && (filter.Filter.PatientId == null || filter.Filter.PatientId == x.PatientId)
                    
                &&(x.IsDeleted == false)
                ) : await Repository.GetAll(x => x.IsDeleted == false);
                entities = entities.OrderBy(x => x.Id * -1).ToList();

                var firstIndex = filter.PageCount * filter.ContentCount;
                var lastIndex = firstIndex + filter.ContentCount;

                lastIndex = Math.Min(lastIndex, entities.Count);
                var values = new List<PatientReportListDto>();
                for (int i = firstIndex; i < lastIndex; i++)
                {
                    values.Add(Mapper.Map<PatientReportListDto>(entities[i]));
                }

                result.Result = new GenericLoadMoreDto<PatientReportListDto>
                {
                    Values = values,
                    ContentCount = filter.ContentCount,
                    NextPage = lastIndex < entities.Count,
                    TotalPageCount = Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount)),
                    TotalContentCount = entities.Count,
                    PageCount = filter.PageCount > Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount))
                    ? Convert.ToInt32(Math.Ceiling(entities.Count / (double)filter.ContentCount))
                    : filter.PageCount,
                    PrevPage = firstIndex > 0


                };

            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientReportPatientReportGetAllExceptionError, ex.Message);
            }
            return result;
        }

        public async Task<BussinessLayerResult<PatientReportListDto>> Update(PatientReportDto patientreport)
        {
            var result = new BussinessLayerResult<PatientReportListDto>();
            try
            {
                var entity = await Repository.Get(patientreport.Id);
                entity.IsDeleted = false;

                entity.UpdateTime = DateTime.Now;


                entity.Title = patientreport.Title;
                entity.Explanation = patientreport.Explanation;
                entity.ReportType = patientreport.ReportType;
                


                var validationResult = await Validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    result.ErrorMessages.AddRange(
                            validationResult.Errors.Select(x =>
                                new ErrorDto
                                {
                                    ErrorCode = EErrorCode.PatientReportPatientReportUpdateValidationError,
                                    Message = x.ErrorMessage
                                }
                             )
                         );
                    return result;
                }

                entity = await Repository.Update(entity);
                result.Result = Mapper.Map<PatientReportListDto>(entity);



            }
            catch (Exception ex)
            {
                result.AddError(EErrorCode.PatientReportPatientReportUpdateExceptionError, ex.Message);

            }
            return result;
        }
    }
}


