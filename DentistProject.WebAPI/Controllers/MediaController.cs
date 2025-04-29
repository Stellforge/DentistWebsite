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
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public MediaController(IMediaService mediaService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _mediaService = mediaService;
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
        //[HttpPost]
        //public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] MediaFilter? filter)
        //{
        //    if (!methods.Contains(EMethod.MediaList))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _mediaService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.MediaFilter>
        //    {
        //        ContentCount = 10,
        //        PageCount = page ?? 0,
        //        Filter = filter

        //    });
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}






        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.MediaGet))
            {
                return Unauthorized();
            }
            var result = await _mediaService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return File(result.Result.File,result.Result.FileType,result.Result.FileName);
            }
            return BadRequest(result.ErrorMessages);
        }


      


        [HttpPost("Add")]
        public async Task<IActionResult> Add(MediaDto media)
        {
            if (!methods.Contains(EMethod.MediaAdd))
            {
                return Unauthorized();
            }
            var result = await _mediaService.Add(media);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(MediaDto media)
        {
            if (!methods.Contains(EMethod.MediaUpdate))
            {
                return Unauthorized();
            }
            var result = await _mediaService.Update(media);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.MediaDelete))
            {
                return Unauthorized();
            }
            var result = await _mediaService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

