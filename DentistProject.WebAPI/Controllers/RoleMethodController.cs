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
    public class RoleMethodController : ControllerBase
    {
        private readonly IRoleMethodService _rolemethodService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public RoleMethodController(IRoleMethodService rolemethodService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _rolemethodService = rolemethodService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] RoleMethodFilter? filter)
        {
            if (!methods.Contains(EMethod.RoleMethodList))
            {
                return Unauthorized();
            }
            var result = await _rolemethodService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.RoleMethodFilter>
            {
                ContentCount = int.MaxValue,
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
        public async Task<IActionResult> Count([FromBody] RoleMethodFilter? filter)
        {
            if (!methods.Contains(EMethod.RoleMethodCount))
            {
                return Unauthorized();
            }
            var result = await _rolemethodService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.RoleMethodGet))
            {
                return Unauthorized();
            }
            var result = await _rolemethodService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.RoleMethodGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _rolemethodService.GetAll(new Dtos.Filter.LoadMoreFilter<RoleMethodFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new RoleMethodFilter()
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
        //public async Task<IActionResult> ChangePhoto(RoleMethodDto rolemethod)
        //{
        //    if (!methods.Contains(EMethod.RoleMethodChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _rolemethodService.ChangePhoto(rolemethod);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate(RoleMethodDto rolemethod)
        {
            if (!methods.Contains(EMethod.RoleMethodAdd))
            {
                return Unauthorized();
            }
            var result = await _rolemethodService.AddOrUpdate(rolemethod);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.RoleMethodDelete))
            {
                return Unauthorized();
            }
            var result = await _rolemethodService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

