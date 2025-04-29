using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.Enum
{
    public enum EErrorCode
    {
        Unknown=0,

        AboutAboutAddValidationError = 101,
        AboutAboutAddExceptionError,
        AboutAboutDeleteExceptionError,
        AboutAboutGetExceptionError,
        AboutAboutCountExceptionError,
        AboutAboutGetAllExceptionError,
        AboutAboutUpdateValidationError,
        AboutAboutUpdateExceptionError,


        AppointmentAppointmentAddValidationError = 131,
        AppointmentAppointmentAddExceptionError,
        AppointmentAppointmentDeleteExceptionError,
        AppointmentAppointmentGetExceptionError,
        AppointmentAppointmentCountExceptionError,
        AppointmentAppointmentGetAllExceptionError,
        AppointmentAppointmentUpdateValidationError,
        AppointmentAppointmentUpdateExceptionError,


        AppointmentRequestAppointmentRequestAddValidationError = 161,
        AppointmentRequestAppointmentRequestAddExceptionError,
        AppointmentRequestAppointmentRequestDeleteExceptionError,
        AppointmentRequestAppointmentRequestGetExceptionError,
        AppointmentRequestAppointmentRequestCountExceptionError,
        AppointmentRequestAppointmentRequestGetAllExceptionError,
        AppointmentRequestAppointmentRequestUpdateValidationError,
        AppointmentRequestAppointmentRequestUpdateExceptionError,


        BlogCategoryBlogCategoryAddValidationError = 191,
        BlogCategoryBlogCategoryAddExceptionError,
        BlogCategoryBlogCategoryDeleteExceptionError,
        BlogCategoryBlogCategoryGetExceptionError,
        BlogCategoryBlogCategoryCountExceptionError,
        BlogCategoryBlogCategoryGetAllExceptionError,
        BlogCategoryBlogCategoryUpdateValidationError,
        BlogCategoryBlogCategoryUpdateExceptionError,


        BlogBlogAddValidationError = 1121,
        BlogBlogAddExceptionError,
        BlogBlogDeleteExceptionError,
        BlogBlogGetExceptionError,
        BlogBlogCountExceptionError,
        BlogBlogGetAllExceptionError,
        BlogBlogUpdateValidationError,
        BlogBlogUpdateExceptionError,


        ContactContactAddValidationError = 1151,
        ContactContactAddExceptionError,
        ContactContactDeleteExceptionError,
        ContactContactGetExceptionError,
        ContactContactCountExceptionError,
        ContactContactGetAllExceptionError,
        ContactContactUpdateValidationError,
        ContactContactUpdateExceptionError,


        DentistDentistAddValidationError = 1181,
        DentistDentistAddExceptionError,
        DentistDentistDeleteExceptionError,
        DentistDentistGetExceptionError,
        DentistDentistCountExceptionError,
        DentistDentistGetAllExceptionError,
        DentistDentistUpdateValidationError,
        DentistDentistUpdateExceptionError,


        DentistSocialDentistSocialAddValidationError = 1211,
        DentistSocialDentistSocialAddExceptionError,
        DentistSocialDentistSocialDeleteExceptionError,
        DentistSocialDentistSocialGetExceptionError,
        DentistSocialDentistSocialCountExceptionError,
        DentistSocialDentistSocialGetAllExceptionError,
        DentistSocialDentistSocialUpdateValidationError,
        DentistSocialDentistSocialUpdateExceptionError,


        IdentityIdentityAddValidationError = 1241,
        IdentityIdentityCheckExceptionError,
        IdentityIdentityDeleteExceptionError,
        IdentityIdentityGetExceptionError,
        IdentityIdentityCountExceptionError,
        IdentityIdentityGetAllExceptionError,
        IdentityIdentityUpdateValidationError,
        IdentityIdentityForgatExceptionError,


        InvoiceInvoiceAddValidationError = 1271,
        InvoiceInvoiceAddExceptionError,
        InvoiceInvoiceDeleteExceptionError,
        InvoiceInvoiceGetExceptionError,
        InvoiceInvoiceCountExceptionError,
        InvoiceInvoiceGetAllExceptionError,
        InvoiceInvoiceUpdateValidationError,
        InvoiceInvoiceUpdateExceptionError,


        MediaMediaAddValidationError = 1301,
        MediaMediaAddExceptionError,
        MediaMediaDeleteExceptionError,
        MediaMediaGetExceptionError,
        MediaMediaCountExceptionError,
        MediaMediaGetAllExceptionError,
        MediaMediaUpdateValidationError,
        MediaMediaUpdateExceptionError,


        MessageMessageAddValidationError = 1331,
        MessageMessageAddExceptionError,
        MessageMessageDeleteExceptionError,
        MessageMessageGetExceptionError,
        MessageMessageCountExceptionError,
        MessageMessageGetAllExceptionError,
        MessageMessageUpdateValidationError,
        MessageMessageUpdateExceptionError,


        OffHoursOffHoursAddValidationError = 1361,
        OffHoursOffHoursAddExceptionError,
        OffHoursOffHoursDeleteExceptionError,
        OffHoursOffHoursGetExceptionError,
        OffHoursOffHoursCountExceptionError,
        OffHoursOffHoursGetAllExceptionError,
        OffHoursOffHoursUpdateValidationError,
        OffHoursOffHoursUpdateExceptionError,


        PatientPatientAddValidationError = 1391,
        PatientPatientAddExceptionError,
        PatientPatientDeleteExceptionError,
        PatientPatientGetExceptionError,
        PatientPatientCountExceptionError,
        PatientPatientGetAllExceptionError,
        PatientPatientUpdateValidationError,
        PatientPatientUpdateExceptionError,


        PatientPrescriptionPatientPrescriptionAddValidationError = 1421,
        PatientPrescriptionPatientPrescriptionAddExceptionError,
        PatientPrescriptionPatientPrescriptionDeleteExceptionError,
        PatientPrescriptionPatientPrescriptionGetExceptionError,
        PatientPrescriptionPatientPrescriptionCountExceptionError,
        PatientPrescriptionPatientPrescriptionGetAllExceptionError,
        PatientPrescriptionPatientPrescriptionUpdateValidationError,
        PatientPrescriptionPatientPrescriptionUpdateExceptionError,


        PatientPrescriptionMedicinePatientPrescriptionMedicineAddValidationError = 1451,
        PatientPrescriptionMedicinePatientPrescriptionMedicineAddExceptionError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineDeleteExceptionError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineGetExceptionError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineCountExceptionError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineGetAllExceptionError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineUpdateValidationError,
        PatientPrescriptionMedicinePatientPrescriptionMedicineUpdateExceptionError,


        PatientReportPatientReportAddValidationError = 1481,
        PatientReportPatientReportAddExceptionError,
        PatientReportPatientReportDeleteExceptionError,
        PatientReportPatientReportGetExceptionError,
        PatientReportPatientReportCountExceptionError,
        PatientReportPatientReportGetAllExceptionError,
        PatientReportPatientReportUpdateValidationError,
        PatientReportPatientReportUpdateExceptionError,


        PatientTreatmentPatientTreatmentAddValidationError = 1511,
        PatientTreatmentPatientTreatmentAddExceptionError,
        PatientTreatmentPatientTreatmentDeleteExceptionError,
        PatientTreatmentPatientTreatmentGetExceptionError,
        PatientTreatmentPatientTreatmentCountExceptionError,
        PatientTreatmentPatientTreatmentGetAllExceptionError,
        PatientTreatmentPatientTreatmentUpdateValidationError,
        PatientTreatmentPatientTreatmentUpdateExceptionError,


        PatientTreatmentServicesPatientTreatmentServicesAddValidationError = 1541,
        PatientTreatmentServicesPatientTreatmentServicesAddExceptionError,
        PatientTreatmentServicesPatientTreatmentServicesDeleteExceptionError,
        PatientTreatmentServicesPatientTreatmentServicesGetExceptionError,
        PatientTreatmentServicesPatientTreatmentServicesCountExceptionError,
        PatientTreatmentServicesPatientTreatmentServicesGetAllExceptionError,
        PatientTreatmentServicesPatientTreatmentServicesUpdateValidationError,
        PatientTreatmentServicesPatientTreatmentServicesUpdateExceptionError,


        ReviewReviewAddValidationError = 1571,
        ReviewReviewAddExceptionError,
        ReviewReviewDeleteExceptionError,
        ReviewReviewGetExceptionError,
        ReviewReviewCountExceptionError,
        ReviewReviewGetAllExceptionError,
        ReviewReviewUpdateValidationError,
        ReviewReviewUpdateExceptionError,


        RoleRoleAddValidationError = 1601,
        RoleRoleAddExceptionError,
        RoleRoleDeleteExceptionError,
        RoleRoleGetExceptionError,
        RoleRoleCountExceptionError,
        RoleRoleGetAllExceptionError,
        RoleRoleUpdateValidationError,
        RoleRoleUpdateExceptionError,


        RoleMethodRoleMethodAddValidationError = 1631,
        RoleMethodRoleMethodAddExceptionError,
        RoleMethodRoleMethodDeleteExceptionError,
        RoleMethodRoleMethodGetExceptionError,
        RoleMethodRoleMethodCountExceptionError,
        RoleMethodRoleMethodGetAllExceptionError,
        RoleMethodRoleMethodUpdateValidationError,
        RoleMethodRoleMethodUpdateExceptionError,


        ServiceServiceAddValidationError = 1661,
        ServiceServiceAddExceptionError,
        ServiceServiceDeleteExceptionError,
        ServiceServiceGetExceptionError,
        ServiceServiceCountExceptionError,
        ServiceServiceGetAllExceptionError,
        ServiceServiceUpdateValidationError,
        ServiceServiceUpdateExceptionError,


        SystemSettingSystemSettingAddValidationError = 1691,
        SystemSettingSystemSettingAddExceptionError,
        SystemSettingSystemSettingDeleteExceptionError,
        SystemSettingSystemSettingGetExceptionError,
        SystemSettingSystemSettingCountExceptionError,
        SystemSettingSystemSettingGetAllExceptionError,
        SystemSettingSystemSettingUpdateValidationError,
        SystemSettingSystemSettingUpdateExceptionError,


        UserUserAddValidationError = 1721,
        UserUserAddExceptionError,
        UserUserDeleteExceptionError,
        UserUserGetExceptionError,
        UserUserCountExceptionError,
        UserUserGetAllExceptionError,
        UserUserUpdateValidationError,
        UserUserUpdateExceptionError,


        UserRoleUserRoleAddValidationError = 1751,
        UserRoleUserRoleAddExceptionError,
        UserRoleUserRoleDeleteExceptionError,
        UserRoleUserRoleGetExceptionError,
        UserRoleUserRoleCountExceptionError,
        UserRoleUserRoleGetAllExceptionError,
        UserRoleUserRoleUpdateValidationError,
        UserRoleUserRoleUpdateExceptionError,


        WorkingHourWorkingHourAddValidationError = 1781,
        WorkingHourWorkingHourAddExceptionError,
        WorkingHourWorkingHourDeleteExceptionError,
        WorkingHourWorkingHourGetExceptionError,
        WorkingHourWorkingHourCountExceptionError,
        WorkingHourWorkingHourGetAllExceptionError,
        WorkingHourWorkingHourUpdateValidationError,
        WorkingHourWorkingHourUpdateExceptionError,
        IdentityIdentityAddExceptionError,
        UserUserChangePhotoExceptionError,
        AboutAboutUpdateMustValidItemError,
        AboutAboutDeleteDontDeleteValidItemError,
        OffHoursOffHoursAddOneOrMoreAppoimentFoundError,
        MediaMediaDeleteItemNotFoundError,
        MediaMediaUpdateItemNotFoundError,
        MediaMediaGetItemNotFoundError,
        AccountForgatPasswordEmailWrongError,
        AccountLogoutPasswordWrongError,
        AccountLoginPasswordWrongError,
        SessionSessionAddValidationError,
        SessionSessionAddExceptionError,
        SessionSessionDeleteExceptionError,
        SessionSessionGetExceptionError,
        SessionSessionGetAllExceptionError,
        SessionSessionUpdateValidationError,
        SessionSessionUpdateExceptionError,
        NotificationNotificationNotifyUserOnEmailExceptionError,
        SystemSettingSystemSettingChangeLogoExceptionError,
        DentistSocialDentistSocialAllUpdateExceptionError,
    }
}
