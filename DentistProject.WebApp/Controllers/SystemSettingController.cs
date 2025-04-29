using AutoMapper;
using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.ListDto;
using DentistProject.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DentistProject.WebApp.Controllers
{
    public class SystemSettingController : Controller
    {
        private readonly ISystemSettingService _systemSettingService;
        private readonly IToastNotification _toastNotification;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        public SystemSettingController(IToastNotification toastNotification, ISystemSettingService systemSettingService, IHttpContextAccessor contextAccessor, IAccountService accountService, IMapper mapper)
        {
            _toastNotification = toastNotification;
            _systemSettingService = systemSettingService;
            _contextAccessor = contextAccessor;
            _accountService = accountService;
            _mapper = mapper;



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


        [HttpPost("/SystemSetting/Logo/{id:long}")]
        public async Task<IActionResult> ChangeLogo([FromForm] LogoDto logo)
        {
            if (!(authMethod.Contains(EMethod.SystemSettingUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _systemSettingService.ChangeLogo(logo);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Redirect("/SystemSetting");
            }
            var message = string.Join("\n", result.ErrorMessages.Select(x => x.Message));
            _toastNotification.AddErrorToastMessage(message);
            return Redirect("/SystemSetting");
        }




        [HttpPost("/SystemSetting/{id:long}")]
        public async Task<IActionResult> Index([FromRoute]long id,[FromForm]SystemSettingDto systemSetting)
        {
            if (!(authMethod.Contains(EMethod.SystemSettingUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            systemSetting.Id= id;
            var result = await _systemSettingService.Update(systemSetting);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Redirect("/SystemSetting");
            }
            var message = string.Join("\n", result.ErrorMessages.Select(x => x.Message));
            _toastNotification.AddErrorToastMessage(message);
            return Redirect("/SystemSetting");
        }


        public async Task<IActionResult> Index()
        {
            if (!(authMethod.Contains(EMethod.SystemSettingList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _systemSettingService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.SystemSettingFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join("\n", result.ErrorMessages.Select(x => x.Message));
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
    }
}
