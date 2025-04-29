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
    public class PatientPrescriptionController : ControllerBase
    {
        private readonly IPatientPrescriptionService _patientprescriptionService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public PatientPrescriptionController(IPatientPrescriptionService patientprescriptionService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _patientprescriptionService = patientprescriptionService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] PatientPrescriptionFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionList))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientPrescriptionFilter>
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
        public async Task<IActionResult> Count([FromBody] PatientPrescriptionFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionCount))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionGet))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.PatientPrescriptionGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientprescriptionService.GetAll(new Dtos.Filter.LoadMoreFilter<PatientPrescriptionFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new PatientPrescriptionFilter()
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
        //public async Task<IActionResult> ChangePhoto(PatientPrescriptionDto patientprescription)
        //{
        //    if (!methods.Contains(EMethod.PatientPrescriptionChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientprescriptionService.ChangePhoto(patientprescription);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(PatientPrescriptionDto patientprescription)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionAdd))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.Add(patientprescription);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PatientPrescriptionDto patientprescription)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionUpdate))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.Update(patientprescription);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionDelete))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

