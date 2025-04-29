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
    public class PatientPrescriptionMedicineController : ControllerBase
    {
        private readonly IPatientPrescriptionMedicineService _patientprescriptionmedicineService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public PatientPrescriptionMedicineController(IPatientPrescriptionMedicineService patientprescriptionmedicineService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _patientprescriptionmedicineService = patientprescriptionmedicineService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] PatientPrescriptionMedicineFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineList))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientPrescriptionMedicineFilter>
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
        public async Task<IActionResult> Count([FromBody] PatientPrescriptionMedicineFilter? filter)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineCount))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineGet))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.PatientPrescriptionMedicineGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientprescriptionmedicineService.GetAll(new Dtos.Filter.LoadMoreFilter<PatientPrescriptionMedicineFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new PatientPrescriptionMedicineFilter()
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
        //public async Task<IActionResult> ChangePhoto(PatientPrescriptionMedicineDto patientprescriptionmedicine)
        //{
        //    if (!methods.Contains(EMethod.PatientPrescriptionMedicineChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _patientprescriptionmedicineService.ChangePhoto(patientprescriptionmedicine);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(PatientPrescriptionMedicineDto patientprescriptionmedicine)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineAdd))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.Add(patientprescriptionmedicine);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PatientPrescriptionMedicineDto patientprescriptionmedicine)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineUpdate))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.Update(patientprescriptionmedicine);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.PatientPrescriptionMedicineDelete))
            {
                return Unauthorized();
            }
            var result = await _patientprescriptionmedicineService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

