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
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ContactController(IContactService contactService, IToastNotification toastNotification, IMapper mapper, IHttpContextAccessor contextAccessor, IAccountService accountService)
        {
            _contactService = contactService;
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
        [HttpGet("ContactList")]
        public async Task<ActionResult> ContactList([FromQuery] int? page)
        {
            if (!(authMethod.Contains(EMethod.ContactList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _contactService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ContactFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.ContactUpdate)) ?? false) || true;
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.ContactDelete)) ?? false) || true;
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("ContactAdd")]
        public async Task<ActionResult> ContactAdd()
        {
            if (!(authMethod.Contains(EMethod.ContactAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var categoryResult = await _contactService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ContactFilter>
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
        [HttpPost("ContactAdd")]
        public async Task<ActionResult> ContactAdd([FromForm] ContactDto contact)
        {
            if (!(authMethod.Contains(EMethod.ContactAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _contactService.Add(contact);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Redirect("/ContactList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(contact);
        }

        [HttpGet("/ContactDelete/{id:long}")]
        public async Task<ActionResult> ContactDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.ContactDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _contactService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/ContactList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }
        [HttpGet("/ContactUpdate/{id:long}")]
        public async Task<IActionResult> ContactUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.ContactUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _contactService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                var categoryResult = await _contactService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.ContactFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                // return View(_mapper.Map<ContactDto>(result.Result)); -> Manuel kısım çalışmazsa yorumu kaldır
                // Manuel Dönüşüm
                var contactDto = new ContactDto
                {
                    Adress = result.Result.Adress,
                    Name = result.Result.Name,
                    Phone1 = result.Result.Phone1,
                    Phone2 = result.Result.Phone2,
                    Email = result.Result.Email,
                    FacebookLink = result.Result.FacebookLink,
                    XLink = result.Result.XLink,
                    YoutubeLink = result.Result.YoutubeLink,
                    InstagramLink = result.Result.InstagramLink,
                    GoogleMapLink = result.Result.GoogleMapLink
                };
                return View(contactDto);
                //
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/ContactUpdate/{id:long}")]
        public async Task<ActionResult> ContactUpdate([FromForm] ContactDto contact)
        {
            if (!(authMethod.Contains(EMethod.ContactUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _contactService.Update(contact);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("ContactList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(contact);
        }
        //
    }
}
