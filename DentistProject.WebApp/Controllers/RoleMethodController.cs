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
    public class RoleMethodController : Controller
    {
        private readonly IRoleMethodService _roleMethodService;


        private long loginUserId = 1;
        private readonly IToastNotification _toastNotification;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAccountService _accountService;

        public RoleMethodController(IRoleMethodService roleMethodService, IToastNotification toastNotification, IMapper mapper, IHttpContextAccessor contextAccessor, IAccountService accountService)
        {
            _roleMethodService = roleMethodService;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _accountService = accountService;

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

        [HttpPost("/RoleMethodUpdate")]
        public async Task<IActionResult> Update(RoleMethodAllUpdateDto roleMethod)
        {
            if (!authMethod.Contains(EMethod.RoleMethodUpdate) || !authMethod.Contains(EMethod.RoleMethodUpdate))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }
            var response = await _roleMethodService.UpdateAll(roleMethod);
            if (response.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("Update");
            }
            var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return View(roleMethod);
        }


        [HttpGet("/RoleMethodUpdate")]
        public async Task<IActionResult> Update()
        {

            if (!authMethod.Contains(EMethod.RoleMethodUpdate) || !authMethod.Contains(EMethod.RoleMethodUpdate))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return Redirect("/");
            }

            var role = Request.Query["role"];
            if (!string.IsNullOrEmpty(role))
            {
                var response = await _roleMethodService.GetAll(new LoadMoreFilter<RoleMethodFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,
                    Filter = new RoleMethodFilter()
                    {
                        Role = (ERoleType)Convert.ToInt32(role)
                    }
                }); ;
                if (response.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.SelectedRole = (ERoleType)Convert.ToInt32(role);
                    ViewBag.SelectedMethods = response.Result.Values.Select(x => x.Method).ToList();

                    return View();
                }
                var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);

            }

            return View();
        }



        [HttpGet("/GetMethods/{roleId:int}")]
        public async Task<List<EMethod>> GetMethodsByRole(ERoleType roleId)
        {
            if (!authMethod.Contains(EMethod.RoleMethodList) || !authMethod.Contains(EMethod.RoleMethodAllList))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamaktadır");
                return new List<EMethod>();
            }
            var response = await _roleMethodService.GetAll(new LoadMoreFilter<RoleMethodFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new RoleMethodFilter()
                {
                    Role = roleId
                }
            });
            if (response.Status == Dtos.Enum.EResultStatus.Success)
            {
                return response.Result.Values.Select(x => x.Method).ToList();
            }
            var message = string.Join(Environment.NewLine, response.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return new List<EMethod>();
        }


    }
}
