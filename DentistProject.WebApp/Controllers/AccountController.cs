using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.ListDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using System.Security.Principal;

namespace DentistProject.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IToastNotification _toastNotification;
        private readonly IHttpContextAccessor _httpContext;
        private long loginUserId = 1;
        private UserListDto? LoginUser;

        public AccountController(IUserService userService, IUserRoleService roleService, IAccountService accountService, IToastNotification toastNotification, IHttpContextAccessor httpContext)
        {
            _accountService = accountService;
            _toastNotification = toastNotification;
            _httpContext = httpContext;
        }




        public IActionResult Index()
        {
            var sessionkey = _httpContext.HttpContext.Request?.Cookies["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success && sessionResult.Result.Result != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] IdentityCheckDto ıdentity)
        {
            ıdentity.DeviceType = Entities.Enum.EDeviceType.Web;
            var result = await _accountService.Login(ıdentity);
            if (result.Status == EResultStatus.Success)
            {
                Response.Cookies.Append("AuthToken", result.Result.Key);
                return Redirect("/");
            }
            var message = string.Join("\n", result.ErrorMessages.Select(e => e.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(ıdentity);
        }



        public async Task<ActionResult> AuthSignup()
        {
            var sessionkey = _httpContext.HttpContext.Request?.Cookies["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success && sessionResult.Result.Result != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost()]
        public async Task<ActionResult> AuthSignup([FromForm] UserDto userDto)
        {
            var result = await _accountService.SignUp(userDto);
            if (result.Status == EResultStatus.Success)
            {
                Response.Cookies.Append("AuthToken", result.Result.Key);
                return Redirect("/");
            }
            var message = string.Join("\n", result.ErrorMessages.Select(e => e.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(userDto);
        }


        public ActionResult AuthRecoverypw()
        {
            var sessionkey = _httpContext.HttpContext.Request?.Cookies["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success && sessionResult.Result.Result != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AuthRecoverypw(IdentityCheckDto ıdentity)
        {
            var result = await _accountService.ForgatPassword(ıdentity.Email);
            if (result.Status == EResultStatus.Success)
            {
                return AuthConfirmmail(ıdentity.Email);
            }
            var message = string.Join("\n", result.ErrorMessages.Select(e => e.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(ıdentity);
        }



        public ActionResult AuthConfirmmail(string Email)
        {
            var sessionkey = _httpContext.HttpContext.Request?.Cookies["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success && sessionResult.Result.Result != null)
            {
                return Redirect("/");
            }
            ViewBag.Email = Email;
            return View();
        }

        public ActionResult LogOut()
        {
            var sessionkey = _httpContext.HttpContext.Request?.Cookies["AuthToken"] ?? "";
            var sessionResult = _accountService.Logout(sessionkey);
            sessionResult.Wait();
            if (sessionResult.Result.Status == Dtos.Enum.EResultStatus.Success)
            {
                _httpContext.HttpContext.Response?.Cookies.Delete("AuthToken");
                return Redirect("/");
            }

            return Redirect("/");
        }


    }
}
