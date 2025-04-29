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
    public class PrescriptionController : Controller
    {
        private readonly IPatientPrescriptionService _patientPrescriptionService;
        private readonly IPatientService _patientService;
        private readonly IDentistService _dentistService;
        private readonly IPatientPrescriptionMedicineService _patientMedicineService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public PrescriptionController(IPatientPrescriptionService patientPrescriptionService, IToastNotification toastNotification, IMapper mapper, IHttpContextAccessor contextAccessor, IAccountService accountService, IPatientService patientService, IPatientPrescriptionMedicineService patientMedicineService, IDentistService dentistService)
        {
            _patientPrescriptionService = patientPrescriptionService;
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
            _patientService = patientService;
            _patientMedicineService = patientMedicineService;
            _dentistService = dentistService;
        }

        // İletişim Kısmı
        [HttpGet("PrescriptionList")]
        public async Task<ActionResult> PrescriptionList([FromQuery] int? page, [FromQuery] string? query)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionList) || authMethod.Contains(EMethod.PatientPrescriptionAllList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            ViewBag.Query=query;
            ViewBag.Page=page;
            var result = await _patientPrescriptionService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientPrescriptionFilter>
            {
                ContentCount = 20,
                PageCount = page ?? 0,
                Filter = new Filters.Filter.PatientPrescriptionFilter
                {
                    DentistUserId = authMethod.Contains(EMethod.PatientPrescriptionAllList) ? null : loginUserId,
                    Search=query   
                }
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.PatientPrescriptionUpdate)) ?? false) || true;
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.PatientPrescriptionDelete)) ?? false) || true;
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("PrescriptionAdd")]
        public async Task<ActionResult> PrescriptionAdd()
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionAdd)))
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
                ViewBag.Patients = result.Result.Values;
            }
            else
            {
                var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }

            var isDentist = (await _dentistService.GetByUserId(loginUserId))?.Result != null;

            ViewBag.IsDentist = isDentist;

            if (!isDentist)
            {
                var result2 = await _dentistService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (result2.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Dentists = result2.Result.Values;
                }
                else
                {
                    var message = string.Join(",", result2.ErrorMessages.Select(x => x.Message).ToList());
                    _toastNotification.AddErrorToastMessage(message);
                }
            }
            else
            {
               ViewBag.Dentist= (await _dentistService.GetByUserId(loginUserId))?.Result;
            }
            return View();
        }
        [HttpPost("PrescriptionAdd")]
        public async Task<ActionResult> PrescriptionAdd([FromForm] PatientPrescriptionDto prescription)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientPrescriptionService.Add(prescription);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("PrescriptionList");
            }
            else
            {
                var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return RedirectToAction("PrescriptionAdd");
        }

        [HttpGet("/PrescriptionDelete/{id:long}")]
        public async Task<ActionResult> PrescriptionDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionDelete) || authMethod.Contains(EMethod.PatientPrescriptionAllDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }

            var result = await _patientPrescriptionService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.Dentist.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi reçetelerinizi silebilirsiniz");
                    return Redirect("/BlogList");
                }
            }
            var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);




            result = await _patientPrescriptionService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/PrescriptionList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();

        }
        [HttpGet("/PrescriptionUpdate/{id:long}")]
        public async Task<IActionResult> PrescriptionUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionUpdate) || authMethod.Contains(EMethod.PatientPrescriptionAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }
            var result = await _patientService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Patients = result.Result.Values;
            }
            else
            {
                var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }

            var isDentist = (await _dentistService.GetByUserId(loginUserId))?.Result != null;

            ViewBag.IsDentist = isDentist;

            if (!isDentist)
            {
                var result2 = await _dentistService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (result2.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Dentists = result2.Result.Values;
                }
                else
                {
                    var message = string.Join(",", result2.ErrorMessages.Select(x => x.Message).ToList());
                    _toastNotification.AddErrorToastMessage(message);
                }
            }
            else
            {
                ViewBag.Dentist = (await _dentistService.GetByUserId(loginUserId))?.Result;
            }

            var result3 = await _patientPrescriptionService.Get(id);
            if (result3.Status == Dtos.Enum.EResultStatus.Success)
            {



                if (result3.Result.Dentist.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı silebilirsiniz");
                    return Redirect("/BlogList");
                }


                var categoryResult = await _patientPrescriptionService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientPrescriptionFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                // return View(_mapper.Map<PrescriptionDto>(result3.Result)); -> Manuel kısım çalışmazsa yorumu kaldır
                // Manuel Dönüşüm
                var prescriptionDto = new PatientPrescriptionDto
                {
                    DentistId = result3.Result.DentistId,
                    PatientId = result3.Result.PatientId
                };
                return View(prescriptionDto);
                //
            }
            var message2 = string.Join(",", result3.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);
            return View();
        }
        [HttpPost("/PrescriptionUpdate/{id:long}")]
        public async Task<ActionResult> PrescriptionUpdate([FromForm] PatientPrescriptionDto prescription)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionUpdate) || authMethod.Contains(EMethod.PatientPrescriptionAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }
           
            var result = await _patientPrescriptionService.Update(prescription);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("PrescriptionList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(prescription);
        }

        [HttpGet("/Prescription/{id:long}")]
        public async Task<IActionResult> PrescriptionDetail(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionGet)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientPrescriptionService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                var medicineResult = await _patientMedicineService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientPrescriptionMedicineFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,
                    Filter = new Filters.Filter.PatientPrescriptionMedicineFilter
                    {
                        PatientPrescriptionId = id
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
    }
}
