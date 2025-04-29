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
    public class DentistSocialController : ControllerBase
    {
        private readonly IDentistSocialService _dentistsocialService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public DentistSocialController(IDentistSocialService dentistsocialService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _dentistsocialService = dentistsocialService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] DentistSocialFilter? filter)
        {
            if (!methods.Contains(EMethod.DentistSocialList))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistSocialFilter>
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
        public async Task<IActionResult> Count([FromBody] DentistSocialFilter? filter)
        {
            if (!methods.Contains(EMethod.DentistSocialCount))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.DentistSocialGet))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.DentistSocialGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _dentistsocialService.GetAll(new Dtos.Filter.LoadMoreFilter<DentistSocialFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new DentistSocialFilter()
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
        //public async Task<IActionResult> ChangePhoto(DentistSocialDto dentistsocial)
        //{
        //    if (!methods.Contains(EMethod.DentistSocialChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _dentistsocialService.ChangePhoto(dentistsocial);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(DentistSocialDto dentistsocial)
        {
            if (!methods.Contains(EMethod.DentistSocialAdd))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.Add(dentistsocial);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DentistSocialDto dentistsocial)
        {
            if (!methods.Contains(EMethod.DentistSocialUpdate))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.Update(dentistsocial);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.DentistSocialDelete))
            {
                return Unauthorized();
            }
            var result = await _dentistsocialService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

