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
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ServiceController(IServiceService serviceService, IToastNotification toastNotification, IMapper mapper, IHttpContextAccessor contextAccessor, IAccountService accountService)
        {
            _serviceService = serviceService;
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

        // İletişim Kısmı
        [HttpGet("ServiceList")]
        public async Task<ActionResult> ServiceList([FromQuery] int? page)
        {
            if (!(authMethod.Contains(EMethod.ServiceList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _serviceService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ServiceFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.ServiceUpdate)) ?? false) || true;
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.ServiceDelete)) ?? false) || true;
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("ServiceAdd")]
        public async Task<ActionResult> ServiceAdd()
        {
            if (!(authMethod.Contains(EMethod.ServiceAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            return View();
        }
        [HttpPost("ServiceAdd")]
        public async Task<ActionResult> ServiceAdd([FromForm] ServiceDto service)
        {
            if (!(authMethod.Contains(EMethod.ServiceAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _serviceService.Add(service);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("ServiceList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(service);
        }

        [HttpGet("/ServiceDelete/{id:long}")]
        public async Task<ActionResult> ServiceDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.ServiceDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _serviceService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/ServiceList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }
        [HttpGet("/ServiceUpdate/{id:long}")]
        public async Task<IActionResult> ServiceUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.ServiceUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _serviceService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                var categoryResult = await _serviceService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ServiceFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                // return View(_mapper.Map<ServiceDto>(result.Result)); -> Manuel kısım çalışmazsa yorumu kaldır
                // Manuel Dönüşüm
                var serviceDto = new ServiceDto
                {
                    Title = result.Result.Title,
                    //Logo = result.Result.Logo,
                    Explanation = result.Result.Explanation,
                    Price = result.Result.Price,
                };
                return View(serviceDto);
                //
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/ServiceUpdate/{id:long}")]
        public async Task<ActionResult> ServiceUpdate([FromForm] ServiceDto service)
        {
            if (!(authMethod.Contains(EMethod.ServiceUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _serviceService.Update(service);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("ServiceList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(service);
        }
    }
}
