using AutoMapper;
using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.Filter;
using DentistProject.Dtos.ListDto;
using DentistProject.Entities.Enum;
using DentistProject.Filters.Filter;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DentistProject.WebApp.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAccountService _accountService;
        public UserRoleController(IUserRoleService userRoleService, IHttpContextAccessor contextAccessor, IAccountService accountService, IToastNotification toastNotification, IUserService userService)
        {
            _userRoleService = userRoleService;
            _contextAccessor = contextAccessor;
            _accountService = accountService;
            _toastNotification = toastNotification;
            _userService = userService;
            var token = _contextAccessor.HttpContext?.Request?.Cookies?["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(token);
            sessionResult.Wait();
            if (sessionResult.Result.Status == EResultStatus.Success)
            {
                loginUserId = sessionResult?.Result?.Result?.UserId ?? -1;
                loginUser = sessionResult?.Result?.Result?.User;
            }
            var methodResult = (loginUserId > 0)
                ? _accountService.GetUserRoleMethods(loginUserId)
                : _accountService.GetPublicRoleMethods();
            methodResult.Wait();
            if (methodResult.Result.Status == EResultStatus.Success)
            {
                authMethod = methodResult.Result.Result;
            }
        }

        public async  Task<IActionResult> Index([FromQuery] int? page)
        {

            if (!authMethod.Contains(EMethod.UserRoleList) )
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }
            var result = await _userService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.UserFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0,
               
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.UserRoleUpdate)) ?? false);
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("/AddUserRole")]
        public async Task<IActionResult> AddUserRole(UserRoleDto roleDto)
        {
            if (!authMethod.Contains(EMethod.UserRoleUpdate) || !authMethod.Contains(EMethod.UserRoleUpdate))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }
            var response = await _userRoleService.Add(roleDto);
            if (response.Status == EResultStatus.Success)
            {
                return Redirect("/ChangeUserRole/"+roleDto.UserId);
            }
            var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return Redirect("/ChangeUserRole/"+ roleDto.UserId);
        }

        [HttpGet("/DeleteUserRole/{userroleId}")]
        public async Task<IActionResult> DeleteUserRole(long userroleId)
        {
            if (!authMethod.Contains(EMethod.UserRoleUpdate) || !authMethod.Contains(EMethod.UserRoleUpdate))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }
            var response = await _userRoleService.Delete(userroleId);
            if (response.Status == EResultStatus.Success)
            {
                return Redirect("/ChangeUserRole/"+response.Result?.UserId);
            }
            var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return RedirectToAction("Index");
        }





        [HttpGet("ChangeUserRole/{userId}")]
        public async Task<IActionResult> ChangeUserRole(long userId)
        {

            if (!authMethod.Contains(EMethod.UserRoleUpdate) || !authMethod.Contains(EMethod.UserRoleUpdate))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }


            var response = await _userRoleService.GetAll(new LoadMoreFilter<UserRoleFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new UserRoleFilter()
                {
                    UserId = userId,
                }
            });
            if (response.Status == EResultStatus.Success)
            {
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.UserRoleDelete)) ?? false);

                ViewBag.SelectedRoles = response.Result.Values.Select(x => x.Role).ToList();
                ViewBag.UserId = userId;
                return View(response.Result);
            }
            var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);



            return View();
        }







    }
}
