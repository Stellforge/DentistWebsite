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
    public class PatientReportController : Controller
    {

        // Hasta Raporu Kısmı
        private readonly IPatientReportService _patientReportService;
        private readonly IPatientService _patientService;
        private readonly IPatientPrescriptionMedicineService _patientMedicineService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public PatientReportController(IPatientReportService patientReportService, IToastNotification toastNotification, IMapper mapper, IPatientReportService patientReportService1, IPatientReportService patientReportService2, IHttpContextAccessor contextAccessor, IAccountService accountService, IPatientService patientService)
        {
            _patientReportService = patientReportService;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _patientReportService = patientReportService;
            _patientReportService = patientReportService;
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
            _patientService = patientService;
        }

        // İletişim Kısmı
        [HttpGet("PatientReportList")]
        public async Task<ActionResult> PatientReportList([FromQuery] int? page, [FromQuery] string? query)
        {
            if (!(authMethod.Contains(EMethod.PatientReportList) || authMethod.Contains(EMethod.PatientReportAllList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/PatientReportUpdate");
            }
            ViewBag.Query = query;
            ViewBag.Page = page;
            var result = await _patientReportService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientReportFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0,
                Filter=new Filters.Filter.PatientReportFilter
                {
                    Search=query
                }
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.PatientReportUpdate)) ?? false) || true;
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.PatientReportDelete)) ?? false) || true;
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("PatientReportAdd")]
        public async Task<ActionResult> PatientReportAdd()
        {
            if (!(authMethod.Contains(EMethod.PatientReportAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Patients = result.Result?.Values;
            }
            else
            {
                var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return View();
        }
        [HttpPost("PatientReportAdd")]
        public async Task<ActionResult> PatientReportAdd([FromForm] PatientReportDto patientReport)
        {
            if (!(authMethod.Contains(EMethod.PatientReportAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientReportService.Add(patientReport);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("PatientReportList");
            }
            else
            {
                var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return View(patientReport);
        }

        [HttpGet("/PatientReportDelete/{id:long}")]
        public async Task<ActionResult> PatientReportDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientReportDelete) || authMethod.Contains(EMethod.PatientReportAllDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/PatientReportList");
            }
            var result = await _patientReportService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.Patient?.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı silebilirsiniz");
                    return Redirect("/PatientReportList");
                }
            }
            var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);

            result = await _patientReportService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/PatientReportList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }
        [HttpGet("/PatientReportUpdate/{id:long}")]
        public async Task<IActionResult> PatientReportUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientReportUpdate) || authMethod.Contains(EMethod.PatientReportAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }

            // Hasta listesini al
            var patientsResult = await _patientService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0
            });

            if (patientsResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Patients = patientsResult.Result?.Values;
            }
            else
            {
                var message2 = string.Join(",", patientsResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message2);
            }
            //

            var result = await _patientReportService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.Patient?.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı silebilirsiniz");
                    return Redirect("/BlogList");
                }
                var categoryResult = await _patientReportService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientReportFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                // return View(_mapper.Map<PrescriptionDto>(result.Result)); -> Manuel kısım çalışmazsa yorumu kaldır
                // Manuel Dönüşüm
                var patientReportDto = new PatientReportDto
                {
                    Title = result.Result.Explanation,
                    Explanation = result.Result.Explanation,
                    ReportType = result.Result.ReportType,
                    PatientId = result.Result.PatientId
                };
                return View(patientReportDto);
                //
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/PatientReportUpdate/{id:long}")]
        public async Task<ActionResult> PatientReportUpdate([FromForm] PatientReportDto patientReport)
        {
            if (!(authMethod.Contains(EMethod.PatientReportUpdate) || authMethod.Contains(EMethod.PatientReportAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }
            var result = await _patientReportService.Update(patientReport);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("PatientReportList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(patientReport);
        }

        [HttpGet("/PatientReportDetail/{id:long}")]
        public async Task<IActionResult> PatientReportDetail(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientReportGet)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientReportService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                var medicineResult = await _patientReportService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientReportFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,
                    Filter = new Filters.Filter.PatientReportFilter
                    {
                        PatientId = id
                    }
                });
                if (medicineResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Medicines = medicineResult.Result;
                }
                else
                {
                    var message3 = string.Join(",", medicineResult.ErrorMessages.Select(x => x.Message).ToList());
                    _toastNotification.AddErrorToastMessage(message3);
                }
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        //
    }
}
