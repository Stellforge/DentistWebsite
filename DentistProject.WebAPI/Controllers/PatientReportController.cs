using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.ListDto;
using DentistProject.Entities.Enum;
using DentistProject.Filters.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentistProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientReportController : ControllerBase
    {
        private readonly IPatientReportService _patientreportService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public PatientReportController(IPatientReportService patientreportService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _patientreportService = patientreportService;
            _accountService = accountService;
            var sessionkey = httpContext.HttpContext.Request?.Cookies["AuthKey"] ?? "";
            var sessionResult = _accountService.GetSession(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success && sessionResult.Result.Result!=null)
            {
                session = sessionResult.Result.Result;
                var methodResult = _accountService.GetUserRoleMethods(session?.UserId??-1);
                methodResult.Wait();
                if (methodResult.Result.Status == Dtos.Enum.EResultStatus.Success)
                {


                    if (methodResult.Result.Result.Count() == 0)
                    {
                        methodResult = _accountService.GetPublicRoleMethods();
                        methodResult.Wait();
                        if (methodResult.Result.Status == Dtos.Enum.EResultStatus.Error)
                        {

                        }
                    }
                    methods = methodResult.Result.Result;
                }
            }
        }


        /// <summary>
        /// getall
        /// </summary>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] PatientReportFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientReportList))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientReportFilter>
            {
                ContentCount = 10,
                PageCount = page ?? 0,
                Filter = filter

            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        [HttpPost("Count")]
        public async Task<IActionResult> Count([FromBody] PatientReportFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientReportCount))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.PatientReportGet))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.PatientReportGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientreportService.GetAll(new Dtos.Filter.LoadMoreFilter<PatientReportFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new PatientReportFilter()
        //        {
        //            IsValid = true
        //        }
        //    });
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result.Values.FirstOrDefault());
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        //[HttpGet("changePhoto")]
        //public async Task<IActionResult> ChangePhoto(PatientReportDto patientreport)
        //{
        //    if (!methods.Contains(EMethod.PatientReportChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientreportService.ChangePhoto(patientreport);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(PatientReportDto patientreport)
        {
            if (!methods.Contains(EMethod.PatientReportAdd))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.Add(patientreport);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PatientReportDto patientreport)
        {
            if (!methods.Contains(EMethod.PatientReportUpdate))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.Update(patientreport);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.PatientReportDelete))
            {
                return Unauthorized();
            }
            var result = await _patientreportService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

