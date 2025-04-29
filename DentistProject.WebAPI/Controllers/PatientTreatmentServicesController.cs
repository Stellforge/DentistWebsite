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
    public class PatientTreatmentServicesController : ControllerBase
    {
        private readonly IPatientTreatmentServicesService _patienttreatmentservicesService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public PatientTreatmentServicesController(IPatientTreatmentServicesService patienttreatmentservicesService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _patienttreatmentservicesService = patienttreatmentservicesService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] PatientTreatmentServicesFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesList))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientTreatmentServicesFilter>
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
        public async Task<IActionResult> Count([FromBody] PatientTreatmentServicesFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesCount))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesGet))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.PatientTreatmentServicesGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patienttreatmentservicesService.GetAll(new Dtos.Filter.LoadMoreFilter<PatientTreatmentServicesFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new PatientTreatmentServicesFilter()
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
        //public async Task<IActionResult> ChangePhoto(PatientTreatmentServicesDto patienttreatmentservices)
        //{
        //    if (!methods.Contains(EMethod.PatientTreatmentServicesChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patienttreatmentservicesService.ChangePhoto(patienttreatmentservices);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(PatientTreatmentServicesDto patienttreatmentservices)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesAdd))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.Add(patienttreatmentservices);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PatientTreatmentServicesDto patienttreatmentservices)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesUpdate))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.Update(patienttreatmentservices);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.PatientTreatmentServicesDelete))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentservicesService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

