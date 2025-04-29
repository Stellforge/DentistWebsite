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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public AppointmentController(IAppointmentService appointmentService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _appointmentService = appointmentService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] AppointmentFilter? filter)
        {
            if (!methods.Contains(EMethod.AppointmentList))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AppointmentFilter>
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
        public async Task<IActionResult> Count([FromBody] AppointmentFilter? filter)
        {
            if (!methods.Contains(EMethod.AppointmentCount))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.AppointmentGet))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.AppointmentGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _appointmentService.GetAll(new Dtos.Filter.LoadMoreFilter<AppointmentFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new AppointmentFilter()
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
        //public async Task<IActionResult> ChangePhoto(AppointmentDto appointment)
        //{
        //    if (!methods.Contains(EMethod.AppointmentChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _appointmentService.ChangePhoto(appointment);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add/{force:bool}")]
        public async Task<IActionResult> Add(AppointmentDto appointment,[FromRoute]bool? force)
        {
            if (!methods.Contains(EMethod.AppointmentAdd))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.Add(appointment, force??false);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        [HttpPost("Update/{force:bool}")]
        public async Task<IActionResult> Update(AppointmentDto appointment, [FromRoute] bool? force)
        {
            if (!methods.Contains(EMethod.AppointmentUpdate))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.Update(appointment,force??false);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.AppointmentDelete))
            {
                return Unauthorized();
            }
            var result = await _appointmentService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

