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
    public class MedicineController : Controller
    {
        private readonly IPatientPrescriptionMedicineService _medicineService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public MedicineController(IPatientPrescriptionMedicineService medicineService, IToastNotification toastNotification, IMapper mapper, IHttpContextAccessor contextAccessor, IAccountService accountService)
        {
            _medicineService = medicineService;
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
        [HttpPost("/MedicineAdd")]
        public async Task<ActionResult> MedicineAdd(PatientPrescriptionMedicineDto medicine2)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _medicineService.Add(medicine2);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return Redirect("/Prescription/"+medicine2.PatientPrescriptionId);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect("/PrescriptionList");
        }
        [HttpGet("/MedicineDelete/{id:long}")]
        public async Task<ActionResult> MedicineDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.PatientPrescriptionAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _medicineService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/Prescription/" + result.Result.PatientPrescriptionId);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect("/PrescriptionList");
        }
    }
}
