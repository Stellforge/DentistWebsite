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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IAccountService _accountService;
        private readonly SessionListDto session;
        private readonly List<EMethod> methods=new List<EMethod>();

        public ContactController(IContactService contactService, IAccountService accountService,IHttpContextAccessor httpContext)
        {
            _contactService = contactService;
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
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromBody] ContactFilter? filter)
        {
            if (!methods.Contains(EMethod.ContactList))
            {
                return Unauthorized();
            }
            var result = await _contactService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ContactFilter>
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
        public async Task<IActionResult> Count([FromBody] ContactFilter? filter)
        {
            if (!methods.Contains(EMethod.ContactCount))
            {
                return Unauthorized();
            }
            var result = await _contactService.Count(filter);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }




        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!methods.Contains(EMethod.ContactGet))
            {
                return Unauthorized();
            }
            var result = await _contactService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }


        [HttpGet("valid")]
        public async Task<IActionResult> GetValid()
        {
            if (!methods.Contains(EMethod.ContactGetValidItem))
            {
                return Unauthorized();
            }
            var result = await _contactService.GetAll(new Dtos.Filter.LoadMoreFilter<ContactFilter>
            {
                ContentCount = 1,
                PageCount = 0,
                Filter = new ContactFilter()
                {
                    Validity = true
                }
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result.Values.FirstOrDefault());
            }
            return BadRequest(result.ErrorMessages);
        }


        //[HttpGet("changePhoto")]
        //public async Task<IActionResult> ChangePhoto(ContactDto contact)
        //{
        //    if (!methods.Contains(EMethod.ContactChangePhoto))
        //    {
        //        return Unauthorized();
        //    }
        //    var result = await _contactService.ChangePhoto(contact);
        //    if (result.Status == Dtos.Enum.EResultStatus.Success)
        //    {
        //        return Ok(result.Result);
        //    }
        //    return BadRequest(result.ErrorMessages);
        //}


        [HttpPost("Add")]
        public async Task<IActionResult> Add(ContactDto contact)
        {
            if (!methods.Contains(EMethod.ContactAdd))
            {
                return Unauthorized();
            }
            var result = await _contactService.Add(contact);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ContactDto contact)
        {
            if (!methods.Contains(EMethod.ContactUpdate))
            {
                return Unauthorized();
            }
            var result = await _contactService.Update(contact);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!methods.Contains(EMethod.ContactDelete))
            {
                return Unauthorized();
            }
            var result = await _contactService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }



    }
}

