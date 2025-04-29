using AutoMapper;
using Azure;
using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Enum;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Principal;

namespace DentistProject.WebApp.Controllers
{
    public class DoktorRandevuController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IDentistService _dentistService;
        private readonly IPatientPrescriptionService _patientPrescriptionService;
        private readonly IDentistSocialService _dentistSocialService;
        private readonly IToastNotification _toastNotification;
        private readonly IAppointmentRequestService _appointmentRequestService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper mapper;
        private long loginUserId = -1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();

        public DoktorRandevuController(IUserService userService, IDentistService dentistService, IPatientService petientService, IPatientPrescriptionService patientPrescriptionService, IDentistSocialService dentistSocialService, IToastNotification toastNotification, IAppointmentRequestService appointmentRequestService, IAppointmentService appointmentService, IAccountService accountService, IHttpContextAccessor contextAccessor, IMapper mapper, IIdentityService identityService)
        {

            _userService = userService;
            _dentistService = dentistService;
            _patientService = petientService;
            _patientPrescriptionService = patientPrescriptionService;
            _dentistSocialService = dentistSocialService;
            _toastNotification = toastNotification;
            _appointmentRequestService = appointmentRequestService;
            _appointmentService = appointmentService;
            _accountService = accountService;
            _identityService = identityService;

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

            this.mapper = mapper;

        }
        public async Task<IActionResult> Index()
        {
            if (!((authMethod.Contains(EMethod.AppointmentRequestList) || authMethod.Contains(EMethod.AppointmentRequestAllList)) && (authMethod.Contains(EMethod.AppointmentList) || authMethod.Contains(EMethod.AppointmentAllList))))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var requestResult = await _appointmentRequestService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AppointmentRequestFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new Filters.Filter.AppointmentRequestFilter
                {
                    DentistUserId = loginUserId,
                }
            });
            if (requestResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Requests = requestResult.Result;
            }
            else
            {
                var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }


            var appoimentResult = await _appointmentService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.AppointmentFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new Filters.Filter.AppointmentFilter
                {
                    UserId = loginUserId,
                }
            });
            if (appoimentResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Appoiments = appoimentResult.Result;
            }
            else
            {
                var message = string.Join(",", appoimentResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }



            return View();
        }

        [HttpPost("/AcceptRequest/{id:long}")]
        public async Task<IActionResult> AcceptRequest(long id, [FromForm] AcceptAppoimentRequestDto acceptAppoimentRequest)
        {
            if (!(authMethod.Contains(EMethod.AppointmentRequestAccept)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var requestResult = await _appointmentRequestService.Get(id);
            if (requestResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (requestResult.Result?.Dentist?.UserId != loginUserId)
                {
                    _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                    return RedirectToAction("Index");

                }
                if (!(requestResult.Result.StartTime < acceptAppoimentRequest.InspactionDate && requestResult.Result.FinishTime > acceptAppoimentRequest.InspactionDate))
                {
                    _toastNotification.AddErrorToastMessage("Lütfen kullanıcının talebine uygun tarih aralığı giriniz");
                    return RedirectToAction("Index");

                }
                var appoimentResult = await _appointmentService.Add(new AppointmentDto
                {
                    AppointmentType = EAppointmentType.Other,
                    AppointmentValidity = EAppointmentValidity.Valid,
                    DentistId = requestResult.Result.DentistId,
                    PatientId = requestResult.Result.PatientId,
                    InspectionDate = acceptAppoimentRequest.InspactionDate,
                    InspectionTimeHour = acceptAppoimentRequest.InspactionTimeHour,
                    Origin = acceptAppoimentRequest.Info
                }, false);

                if (appoimentResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    await _appointmentRequestService.Delete(id);
                }
                else
                {
                    var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                    _toastNotification.AddErrorToastMessage(message);
                }
            }
            else
            {
                var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return RedirectToAction("Index");
        }


        [HttpPost("/AcceptRequestForce/{id:long}")]
        public async Task<IActionResult> AcceptRequestForce(long id, [FromForm] AcceptAppoimentRequestDto acceptAppoimentRequest)
        {
            if (!(authMethod.Contains(EMethod.AppointmentRequestAccept)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var requestResult = await _appointmentRequestService.Get(id);
            if (requestResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (requestResult.Result?.Dentist?.UserId != loginUserId)
                {
                    _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                    return RedirectToAction("Index");
                }
                var appoimentResult = await _appointmentService.Add(new AppointmentDto
                {
                    AppointmentType = EAppointmentType.Other,
                    AppointmentValidity = EAppointmentValidity.Valid,
                    DentistId = requestResult.Result.DentistId,
                    PatientId = requestResult.Result.PatientId,
                    InspectionDate = acceptAppoimentRequest.InspactionDate,
                    InspectionTimeHour = acceptAppoimentRequest.InspactionTimeHour,
                    Origin = acceptAppoimentRequest.Info
                }, true);

                if (appoimentResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    await _appointmentRequestService.Delete(id);
                }
                else
                {
                    var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                    _toastNotification.AddErrorToastMessage(message);
                }
            }
            else
            {
                var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/RejectRequest/{id:long}")]
        public async Task<IActionResult> RejectRequest(long id)
        {
            if (!(authMethod.Contains(EMethod.AppointmentRequestReject)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var requestResult = await _appointmentRequestService.Get(id);
            if (requestResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (requestResult.Result?.Dentist?.UserId != loginUserId)
                {
                    _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                    return RedirectToAction("Index");
                }

                await _appointmentRequestService.Delete(id);

            }
            else
            {
                var message = string.Join(",", requestResult.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message);
            }
            return RedirectToAction("Index");
        }









        public async Task<ActionResult> Profile()
        {
            var dentistResult = await _dentistService.GetByUserId(loginUserId);
            if (dentistResult.Status == Dtos.Enum.EResultStatus.Success && dentistResult.Result != null)
            {
                if (dentistResult != null)
                {
                    ViewBag.Dentist = dentistResult.Result;
                    var patientCountResult = await _patientPrescriptionService.Count(new Filters.Filter.PatientPrescriptionFilter
                    {
                        DentistId = dentistResult.Result.Id
                    });
                    if (patientCountResult.Status == Dtos.Enum.EResultStatus.Success)
                    {
                        ViewBag.PatientCount = patientCountResult.Result;
                    }

                    var dentistSocialResult = await _dentistSocialService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistSocialFilter>
                    {
                        ContentCount = int.MaxValue,
                        PageCount = 0,
                        Filter = new Filters.Filter.DentistSocialFilter
                        {
                            DentistId = dentistResult.Result.Id,

                        }
                    });
                    if (dentistResult.Status == Dtos.Enum.EResultStatus.Success)
                    {
                        ViewBag.DentistSocialList = dentistSocialResult.Result.Values;
                    }
                }
            }
            var patientResult = await _patientService.GetByUserId(loginUserId);
            if (patientResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (patientResult != null)
                {
                    ViewBag.Patient = patientResult.Result;
                }
            }

            return View(loginUser);
        }



        public async Task<ActionResult> ProfileSettings()
        {

            var dentistResult = await _dentistService.GetByUserId(loginUserId);
            if (dentistResult.Status == Dtos.Enum.EResultStatus.Success && dentistResult.Result != null)
            {
                if (dentistResult != null)
                {
                    var dentistSocialResult = await _dentistSocialService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistSocialFilter>
                    {
                        ContentCount = int.MaxValue,
                        PageCount = 0,
                        Filter = new Filters.Filter.DentistSocialFilter
                        {
                            DentistId = dentistResult.Result.Id,

                        }
                    });
                    if (dentistResult.Status == Dtos.Enum.EResultStatus.Success)
                    {
                        ViewBag.DentistSocialList = dentistSocialResult.Result.Values;
                    }
                    return View(mapper.Map<DentistDto>(dentistResult.Result));
                }
                else
                {
                    return RedirectToAction("UserProfileSettings");
                }
            }


            return RedirectToAction("UserProfileSettings");
        }
        public async Task<ActionResult> UserProfileSettings()
        {

            var result = await _userService.Get(loginUserId);
            if (result.Status == Dtos.Enum.EResultStatus.Success && result.Result != null)
            {
                if (result != null)
                {
                    return View(mapper.Map<UserDto>(result.Result));
                }
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UserProfileSettings(UserDto user)
        {
            user.Id = loginUserId;
            var result = await _userService.Update(user);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Başarılı");
                return View(mapper.Map<UserDto>(result.Result));
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> ProfileSettings([FromForm] DentistDto dentist)
        {

            if (dentist == null)
            {
                //Todo: Hata verdirilecek
                return View();
            }

            var result = await _dentistService.Update(dentist);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("Profile");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/ProfileSettingsSocial")]
        public async Task<ActionResult> ProfileSettingsSocial([FromForm] DentistSocialAddDto dentistSocial)
        {
            if (dentistSocial == null)
            {
                //Todo: Hata verdirilecek
                return View();
            }

            var result = await _dentistSocialService.Update(dentistSocial);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("Profile");
            }

            return View();
        }




        [HttpPost("/AddPatient/")]
        public async Task<ActionResult> AddPatient([FromForm] PatientDto patient)
        {
            if (!authMethod.Contains(EMethod.PatientAdd))
            {
                _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientService.Add(patient);
            if (result.Status == EResultStatus.Success)
            {
                return RedirectToAction("PatientList");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("/AddPatient/")]
        public ActionResult AddPatient()
        {
            if (!authMethod.Contains(EMethod.PatientAdd))
            {
                _toastNotification.AddErrorToastMessage("Yetkiniz yok");

                return Redirect("/");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> PatientList()
        {
            if (!(authMethod.Contains(EMethod.PatientList) || authMethod.Contains(EMethod.PatientAllList)))
            {
                _toastNotification.AddErrorToastMessage("Yetkiniz yok");

                return Redirect("/");
            }

            var patientListResult = await _patientService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.PatientFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (patientListResult.Status == EResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod?.Contains(EMethod.PatientUpdate));
                ViewBag.CanDelete = (authMethod?.Contains(EMethod.PatientDelete));

                return View(patientListResult.Result);
            }
            if (patientListResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.IDesntistService = patientListResult.Result.Values;
            }
            return View(patientListResult.Result);

        }

        [HttpGet("/PatientDelete/{id:long}")]
        public async Task<ActionResult> PatientDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.PatientDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece Yetkili Kişi Silebilir");
                    return Redirect("/");
                }
            }
            else
            {
                var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message2);
            }
            result = await _patientService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/DoktorRandevu/PatientList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }


        [HttpGet("/PatientUpdate/{id:long}")]
        public async Task<IActionResult> PatientUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }

            var result = await _patientService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.PatientUpdate)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece Yetkili Kişi Değiştirebilir");
                    return Redirect("/");
                }
              
                return View(mapper.Map<PatientDto>(result.Result));
                // Manuel Dönüşüm -> Manuel kısım çalışmazsa yorumu kaldır
                ////////var DentistDto = new PatientReportDto
                ////////{
                ////////    Title = result.Result.Explanation,
                ////////    Explanation = result.Result.Explanation,
                ////////    ReportType = result.Result.ReportType,
                ////////    PatientId = result.Result.PatientId
                ////////};
                //return View(patientReportDto);
                ////
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/PatientUpdate/{id:long}")]
        public async Task<ActionResult> PatientUpdate([FromForm] PatientDto patient)
        {
            if (!(authMethod.Contains(EMethod.PatientUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _patientService.Update(patient);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("PatientList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(patient);
        }


        [HttpPost("/AddDoctor/")]
        public async Task<IActionResult> AddDoctor([FromForm] DentistDto dentist)
        {
            if (!authMethod.Contains(EMethod.DentistAdd))
            {
                return Redirect("/");
            }
            var result = await _dentistService.Add(dentist);
            if (result.Status == EResultStatus.Success)
            {
                return RedirectToAction("DoctorList");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("DoctorList");
        }

        [HttpGet("/AddDoctor/")]
        public ActionResult AddDoctor()
        {
            if (!authMethod.Contains(EMethod.DentistAdd))
            {
                _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            return View();
        }


        [HttpGet("/DoktorRandevu/DoctorList")]
        public async Task<ActionResult> DoctorList()
        {
            if (!authMethod.Contains(EMethod.DentistList))
            {
                _toastNotification.AddErrorToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var dentistListResult = await _dentistService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.DentistFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
            });
            if (dentistListResult.Status == EResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod?.Contains(EMethod.DentistUpdate));
                ViewBag.CanDelete = (authMethod?.Contains(EMethod.DentistDelete));

                return View(dentistListResult.Result);
            }
            if (dentistListResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.IDesntistService = dentistListResult.Result.Values;
            }
            return View(dentistListResult.Result);
        }

        [HttpGet("/DoctorDelete/{id:long}")]
        public async Task<ActionResult> DoctorDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.DentistDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _dentistService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.DentistDelete)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece Yetkili Kişi Silebilir");
                    return Redirect("/");
                }
            }
            else
            {
                var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
                _toastNotification.AddErrorToastMessage(message2);
            }
            result = await _dentistService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/DoktorRandevu/DoctorList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("/DoctorUpdate/{id:long}")]
        public async Task<IActionResult> DoctorUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.DentistUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }

            var result = await _dentistService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.DentistUpdate)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece Yetkili Kişi Değiştirebilir");
                    return Redirect("/");
                }

                return View(mapper.Map<DentistDto>(result.Result));
                ////Manuel Dönüşüm -> Manuel kısım çalışmazsa yorumu kaldır
                //var DentistDto = new DentistDto
                //{
                //    Photo = result.Result.Photo,
                //    Role = result.Result.User.r ,
                //    ReportType = result.Result.ReportType,
                //    PatientId = result.Result.PatientId
                //};
                //return View(DentistDto);
                ////
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("/DoctorUpdate/{id:long}")]
        public async Task<ActionResult> DoctorUpdate([FromForm] DentistDto dentist)
        {
            if (!(authMethod.Contains(EMethod.DentistUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _dentistService.Update(dentist);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("DoctorList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(dentist);
        }







        public ActionResult DoctorEkle()
        {
            if (!authMethod.Contains(EMethod.DentistAdd))
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult DoctorSil()
        {
            if (!authMethod.Contains(EMethod.DentistDelete))
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult DoctorGuncelle()
        {
            //Todo: Ne işe yarıyor
            return View();
        }
        //
        public ActionResult CalenderConnections()
        {
            return View();

        }
        [HttpGet("Integration")]
        public ActionResult Integration()
        {
            return View();
        }
        [HttpGet("PrivacyPolicy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromForm] IdentityDto identity)
        {
            if (!authMethod.Contains(EMethod.UserUpdate))
            {
                return Redirect("/");
            }
            if (identity.Password != identity.ConfirmPassword)
            {
                _toastNotification.AddAlertToastMessage("Girdiğiniz şifreler uyuşmuyor");
                return View(identity);
            }
            identity.UserId = loginUserId;
            var result = await _identityService.ChangePassword(identity);
            if (result.Status == EResultStatus.Success)
            {

                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View("/DoktorRandevu/ProfileSettings/");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProfilePhotoChange([FromForm] UserDto User)
        {
            if (!authMethod.Contains(EMethod.UserUpdate) || !authMethod.Contains(EMethod.DentistChangePhoto) || !authMethod.Contains(EMethod.UserChangePhoto))
            {
                return Redirect("/");
            }
            User.Id = loginUserId;
            var result = await _userService.ChangePhoto(User);
            if (result.Status == EResultStatus.Success)
            {
                return RedirectToAction("ProfileSettings");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("ChangePhoto", loginUser.Id);
        }

        [HttpGet]
        public ActionResult ProfilePhotoChange()
        {
            return View();
        }



    }
}
