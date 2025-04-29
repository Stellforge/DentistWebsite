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
    public class OffHoursController : ControllerBase
    {
        private readonly IOffHoursService _offhoursService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public OffHoursController(IOffHoursService offhoursService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _offhoursService = offhoursService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] OffHoursFilter? filter)
        {
            if (!methods.Contains(EMethod.OffHoursList))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.OffHoursFilter>
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
        public async Task<IActionResult> Count([FromBody] OffHoursFilter? filter)
        {
            if (!methods.Contains(EMethod.OffHoursCount))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.OffHoursGet))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.OffHoursGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _offhoursService.GetAll(new Dtos.Filter.LoadMoreFilter<OffHoursFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new OffHoursFilter()
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
        //public async Task<IActionResult> ChangePhoto(OffHoursDto offhours)
        //{
        //    if (!methods.Contains(EMethod.OffHoursChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _offhoursService.ChangePhoto(offhours);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add/{force:bool}")]
        public async Task<IActionResult> Add(OffHoursDto offhours, [FromRoute] bool? force)
        {
            if (!methods.Contains(EMethod.OffHoursAdd))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.Add(offhours,force??false);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(OffHoursDto offhours)
        {
            if (!methods.Contains(EMethod.OffHoursUpdate))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.Update(offhours);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.OffHoursDelete))
            {
                return Unauthorized();
            }
            var result = await _offhoursService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

