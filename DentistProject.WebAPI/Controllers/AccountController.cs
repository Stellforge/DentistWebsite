using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentistProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(IdentityCheckDto identity)
        {
            var result = await _accountService.Login(identity);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                Response.Cookies.Append("AuthKey", result.Result.Key);
                var methods = await _accountService.GetUserRoleMethods(result.Result.UserId);
                if (methods?.Result.Count() == 0)
                {
                     methods = await _accountService.GetPublicRoleMethods();
                }
                return Ok(new { methods, result.Result.Key });

            }
            return BadRequest(result.ErrorMessages);
        }
    }
}
