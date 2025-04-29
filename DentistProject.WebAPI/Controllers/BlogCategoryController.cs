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
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _blogcategoryService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public BlogCategoryController(IBlogCategoryService blogcategoryService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _blogcategoryService = blogcategoryService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] BlogCategoryFilter? filter)
        {
            if (!methods.Contains(EMethod.BlogCategoryList))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.BlogCategoryFilter>
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
        public async Task<IActionResult> Count([FromBody] BlogCategoryFilter? filter)
        {
            if (!methods.Contains(EMethod.BlogCategoryCount))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.BlogCategoryGet))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("valid")]
        //public async Task<IActionResult> GetValid()
        //{
        //    if (!methods.Contains(EMethod.BlogCategoryGetValidItem))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _blogcategoryService.GetAll(new Dtos.Filter.LoadMoreFilter<BlogCategoryFilter>
        //    {
        //        ContentCount = 1,
        //        PageCount = 0,
        //        Filter = new BlogCategoryFilter()
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
        //public async Task<IActionResult> ChangePhoto(BlogCategoryDto blogcategory)
        //{
        //    if (!methods.Contains(EMethod.BlogCategoryChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _blogcategoryService.ChangePhoto(blogcategory);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(BlogCategoryDto blogcategory)
        {
            if (!methods.Contains(EMethod.BlogCategoryAdd))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.Add(blogcategory);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(BlogCategoryDto blogcategory)
        {
            if (!methods.Contains(EMethod.BlogCategoryUpdate))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.Update(blogcategory);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.BlogCategoryDelete))
            {
                return Unauthorized();
            }
            var result = await _blogcategoryService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

