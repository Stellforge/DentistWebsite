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
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IToastNotification _toastNotification;
        private UserListDto? loginUser;
        private long loginUserId = -1;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AboutController(IAboutService aboutService, IToastNotification toastNotification, IMapper mapper, IAccountService accountService, IHttpContextAccessor contextAccessor)
        {
            _aboutService = aboutService;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _accountService = accountService;
            _contextAccessor = contextAccessor;
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

        // Hakkımda Kısmı
        [HttpGet("AboutList")]
        public async Task<ActionResult> AboutList([FromQuery] int? page)
        {
            if (!(authMethod.Contains(EMethod.AboutList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _aboutService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AboutFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.AboutUpdate)) ?? false) || true;
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.AboutDelete)) ?? false) || true;
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("AboutAdd")]
        public async Task<ActionResult> AboutAdd()
        {
            if (!(authMethod.Contains(EMethod.AboutAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var categoryResult = await _aboutService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AboutFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
            });
            if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Categories = categoryResult.Result?.Values;
            }
            return View();
        }
        [HttpPost("AboutAdd")]
        public async Task<ActionResult> AboutAdd([FromForm] AboutDto about)
        {
            if (!(authMethod.Contains(EMethod.AboutAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _aboutService.Add(about);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("AboutList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(about);
        }
        [HttpGet("/AboutDelete/{id:long}")]
        public async Task<ActionResult> AboutDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.AboutDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _aboutService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/AboutList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }
        [HttpGet("/AboutUpdate/{id:long}")]
        public async Task<IActionResult> AboutUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.AboutUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _aboutService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                var categoryResult = await _aboutService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AboutFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                // return View(_mapper.Map<AboutDto>(result.Result)); - Manuel Dönüşüm olmazsa yorumu kaldır
                //Manuel Dönüşüm
                var aboutDto = new AboutDto
                {
                    IsValid = result.Result.IsValid,
                    OurWork = result.Result.OurWork,
                    MissionStatement = result.Result.MissionStatement,
                    Target = result.Result.Target,
                    Explanation = result.Result.Explanation
                };
                return View(aboutDto);

                //
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/AboutUpdate/{id:long}")]
        public async Task<ActionResult> AboutUpdate([FromForm] AboutDto about)
        {
            if (!(authMethod.Contains(EMethod.AboutUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _aboutService.Update(about);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("AboutList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(about);
        }
        //
    }
}
