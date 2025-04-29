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
    public class AppointmentRequestController : ControllerBase
    {
        private readonly IAppointmentRequestService _appointmentrequestService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public AppointmentRequestController(IAppointmentRequestService appointmentrequestService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _appointmentrequestService = appointmentrequestService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] AppointmentRequestFilter? filter)
        {
            if (!methods.Contains(EMethod.AppointmentRequestList))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AppointmentRequestFilter>
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
        public async Task<IActionResult> Count([FromBody] AppointmentRequestFilter? filter)
        {
            if (!methods.Contains(EMethod.AppointmentRequestCount))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.AppointmentRequestGet))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.AppointmentRequestGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _appointmentrequestService.GetAll(new Dtos.Filter.LoadMoreFilter<AppointmentRequestFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new AppointmentRequestFilter()
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
        //public async Task<IActionResult> ChangePhoto(AppointmentRequestDto appointmentrequest)
        //{
        //    if (!methods.Contains(EMethod.AppointmentRequestChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _appointmentrequestService.ChangePhoto(appointmentrequest);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(AppointmentRequestDto appointmentrequest)
        {
            if (!methods.Contains(EMethod.AppointmentRequestAdd))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.Add(appointmentrequest);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AppointmentRequestDto appointmentrequest)
        {
            if (!methods.Contains(EMethod.AppointmentRequestUpdate))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.Update(appointmentrequest);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.AppointmentRequestDelete))
            {
                return Unauthorized();
            }
            var result = await _appointmentrequestService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

