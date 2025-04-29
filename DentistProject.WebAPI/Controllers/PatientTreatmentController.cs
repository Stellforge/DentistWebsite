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
    public class PatientTreatmentController : ControllerBase
    {
        private readonly IPatientTreatmentService _patienttreatmentService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public PatientTreatmentController(IPatientTreatmentService patienttreatmentService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _patienttreatmentService = patienttreatmentService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] PatientTreatmentFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientTreatmentList))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientTreatmentFilter>
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
        public async Task<IActionResult> Count([FromBody] PatientTreatmentFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientTreatmentCount))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.PatientTreatmentGet))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.PatientTreatmentGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patienttreatmentService.GetAll(new Dtos.Filter.LoadMoreFilter<PatientTreatmentFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new PatientTreatmentFilter()
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
        //public async Task<IActionResult> ChangePhoto(PatientTreatmentDto patienttreatment)
        //{
        //    if (!methods.Contains(EMethod.PatientTreatmentChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patienttreatmentService.ChangePhoto(patienttreatment);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(PatientTreatmentDto patienttreatment)
        {
            if (!methods.Contains(EMethod.PatientTreatmentAdd))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.Add(patienttreatment);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PatientTreatmentDto patienttreatment)
        {
            if (!methods.Contains(EMethod.PatientTreatmentUpdate))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.Update(patienttreatment);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.PatientTreatmentDelete))
            {
                return Unauthorized();
            }
            var result = await _patienttreatmentService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

