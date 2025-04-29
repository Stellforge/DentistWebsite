using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DisciProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IServiceService _serviceService;
        private readonly IDentistService _dentistService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IContactService _contactService;
        private readonly IMessageService _messageService;
        private readonly IAppointmentRequestService _appointmentRequestService;


        private readonly IToastNotification _toastNotification;

        public HomeController(IAboutService aboutService, IToastNotification toastNotification, IServiceService serviceService, IDentistService dentistService, IBlogService blogService, IBlogCategoryService blogCategoryService, IContactService contactService, IMessageService messageService, IAppointmentRequestService appointmentRequestService)
        {
            _aboutService = aboutService;
            _toastNotification = toastNotification;
            _serviceService = serviceService;
            _dentistService = dentistService;
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _contactService = contactService;
            _messageService = messageService;
            _appointmentRequestService = appointmentRequestService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["PageId"] = "1";
            var dentistResult = await _dentistService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.DentistFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (dentistResult.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Dentists = dentistResult.Result;
            }
            var blogResult = await _blogService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.BlogFilter>
            {
                ContentCount = 3,
                PageCount = 0,
                Filter = new DentistProject.Filters.Filter.BlogFilter
                {
                    OnAir = true,
                }

            });
            if (blogResult.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Blogs = blogResult.Result;
            }
            return View();
            //return Redirect("/DoktorRandevu");
        }
        [HttpGet("/About")]
        public async Task<IActionResult> About()
        {
            ViewData["PageId"] = "2";
            var result = await _aboutService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0,
                Filter = new DentistProject.Filters.Filter.AboutFilter
                {
                    IsValid = true,
                }
            });
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success || result.Result?.Values?.Count > 0)
            {
                return View(result.Result?.Values.FirstOrDefault());
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("/Services")]
        public async Task<IActionResult> Services()
        {
            ViewData["PageId"] = "3";
            var result = await _serviceService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.ServiceFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("/Doctors")]
        public async Task<IActionResult> Doctors()
        {
            ViewData["PageId"] = "4";
            var result = await _dentistService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.DentistFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
            return View();
        }
        [HttpGet("/Blog")]
        public async Task<IActionResult> Blog([FromQuery] int? page, [FromQuery] int? category, [FromQuery] string? search)
        {
            ViewData["PageId"] = "5";
            var result = await _blogService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.BlogFilter>
            {
                ContentCount = 2,
                PageCount = page ?? 0,
                Filter = new DentistProject.Filters.Filter.BlogFilter
                {
                    OnAir = true,
                    CategoryId = category,
                    Search = search

                }

            });
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CategoryId = category;
                ViewBag.Search = search;
                var result2 = await _blogCategoryService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.BlogCategoryFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (result2.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = result2.Result;
                }


                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }

        [HttpGet("/Blog/{id:long}")]
        public async Task<IActionResult> BlogDetail(long id)
        {
            ViewData["PageId"] = "5";
            var result = await _blogService.Get(id);
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                var result2 = await _blogCategoryService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.BlogCategoryFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (result2.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = result2.Result;
                }
                return View(result.Result);
            }
            return View();
        }

        [HttpGet("/Contact")]
        public async Task<IActionResult> Contact()
        {
            ViewData["PageId"] = "6";
            var result = await _contactService.GetAll(new DentistProject.Dtos.Filter.LoadMoreFilter<DentistProject.Filters.Filter.ContactFilter>
            {
                ContentCount = 1,
                PageCount = 0,
                Filter = new DentistProject.Filters.Filter.ContactFilter
                {
                    Validity = true,
                }

            });
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                return View(result.Result.Values?.FirstOrDefault());
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }


        [HttpPost("/Message/Add")]
        public async Task<IActionResult> MessageAdd([FromForm] MessageDto messageDto)
        {
            messageDto.Status = "";
            var result = await _messageService.Add(messageDto);
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Mesaj Gönderildi");
                return Redirect("/Contact");
            }
            var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);
            return Redirect("/Contact");
        }



        [HttpGet("Faq")]
        public ActionResult Faq()
        {
            return View();
        }


        [HttpPost("/AppoimentRequest/Add")]
        public async Task<IActionResult> AppoimentRequestAdd(AppointmentRequestDto appointmentRequest)
        {
            appointmentRequest.Patient.User.Address = "";
            appointmentRequest.Patient.User.Password = "12345";
            appointmentRequest.Patient.User.ConfirmPassword = "12345";
            appointmentRequest.Message = appointmentRequest.Message??"";
            appointmentRequest.Patient.IdentityNumber = appointmentRequest.Patient.IdentityNumber??"";
            appointmentRequest.Patient.Phone2 = appointmentRequest.Patient.Phone2??"";

            var result = await _appointmentRequestService.Add(appointmentRequest);
            if (result.Status == DentistProject.Dtos.Enum.EResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Talebiniz Alınmıştır");
                return Redirect("/");
            }
            var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);
            return Redirect("/");
        }



    }
}