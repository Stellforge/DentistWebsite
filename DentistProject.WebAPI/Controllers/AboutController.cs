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
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public AboutController(IAboutService aboutService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _aboutService = aboutService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] AboutFilter? filter)
        {
            if (!methods.Contains(EMethod.AboutList))
            {
                return Unauthorized();
            }
            var result = await _aboutService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AboutFilter>
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
        public async Task<IActionResult> Count([FromBody] AboutFilter? filter)
        {
            if (!methods.Contains(EMethod.AboutCount))
            {
                return Unauthorized();
            }
            var result = await _aboutService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.AboutGet))
            {
                return Unauthorized();
            }
            var result = await _aboutService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        [HttpGet("valid")]
        public async Task<IActionResult> GetValid()
        {
            if (!methods.Contains(EMethod.AboutGetValidItem))
            {
                return Unauthorized();
            }
            var result = await _aboutService.GetAll(new Dtos.Filter.LoadMoreFilter<AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0,
                Filter = new AboutFilter()
                {
                    IsValid = true
                }
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result.Values.FirstOrDefault());
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("changePhoto")]
        //public async Task<IActionResult> ChangePhoto(AboutDto about)
        //{
        //    if (!methods.Contains(EMethod.AboutChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _aboutService.ChangePhoto(about);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(AboutDto about)
        {
            if (!methods.Contains(EMethod.AboutAdd))
            {
                return Unauthorized();
            }
            var result = await _aboutService.Add(about);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AboutDto about)
        {
            if (!methods.Contains(EMethod.AboutUpdate))
            {
                return Unauthorized();
            }
            var result = await _aboutService.Update(about);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.AboutDelete))
            {
                return Unauthorized();
            }
            var result = await _aboutService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}
