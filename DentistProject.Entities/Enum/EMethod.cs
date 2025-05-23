﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities.Enum
{
    public enum EMethod
    {
        None,

        AboutAdd = 10,
        AboutUpdate,
        AboutGet,
        AboutDelete,
        AboutList,
        AboutCount,
        AboutGetValidItem,
        //AboutChangePhoto,
        //AboutChangeLogo,
        //AboutChangeValid,

        AppointmentAdd = 60,
        AppointmentUpdate,
        AppointmentGet,
        AppointmentDelete,
        AppointmentList,
        AppointmentCount,
        AppointmentAllList,
        //AppointmentGetValidItem,
        //AppointmentChangePhoto,
        //AppointmentChangeLogo,
        //AppointmentChangeValid,



        BlogCategoryAdd = 110,
        BlogCategoryUpdate,
        BlogCategoryGet,
        BlogCategoryDelete,
        BlogCategoryList,
        BlogCategoryCount,
        //BlogCategoryGetValidItem,
        //BlogCategoryChangePhoto,
        //BlogCategoryChangeLogo,
        //BlogCategoryChangeValid,

        BlogAdd = 160,
        BlogUpdate,
        BlogGet,
        BlogDelete,
        BlogList,
        BlogCount,
        BlogAllList,
        BlogAllUpdate,
        BlogAllDelete,
        //BlogGetValidItem,
        //BlogChangePhoto,
        //BlogChangeLogo,
        //BlogChangeValid,

        ContactAdd = 210,
        ContactUpdate,
        ContactGet,
        ContactDelete,
        ContactList,
        ContactCount,
        ContactGetValidItem,
        //ContactChangePhoto,
        //ContactChangeLogo,
        //ContactChangeValid,

        DentistAdd = 260,
        DentistUpdate,
        DentistGet,
        DentistDelete,
        DentistList,
        DentistCount,
        //DentistGetValidItem,
        DentistChangePhoto,
        //DentistChangeLogo,
        //DentistChangeValid,

        DentistSocialAdd = 310,
        DentistSocialUpdate,
        DentistSocialGet,
        DentistSocialDelete,
        DentistSocialList,
        DentistSocialCount,
        //DentistSocialGetValidItem,
        //DentistSocialChangePhoto,
        //DentistSocialChangeLogo,
        //DentistSocialChangeValid,

        IdentityAdd = 360,
        IdentityUpdate,
        IdentityGet,
        IdentityDelete,
        IdentityList,
        IdentityCount,
        //IdentityGetValidItem,
        //IdentityChangePhoto,
        //IdentityChangeLogo,
        //IdentityChangeValid,

        InvoiceAdd = 410,
        InvoiceUpdate,
        InvoiceGet,
        InvoiceDelete,
        InvoiceList,
        InvoiceCount,
        //InvoiceGetValidItem,
        //InvoiceChangePhoto,
        //InvoiceChangeLogo,
        //InvoiceChangeValid,

        MediaAdd = 460,
        MediaUpdate,
        MediaGet,
        MediaDelete,
        MediaList,
        MediaCount,
        //MediaGetValidItem,
        //MediaChangePhoto,
        //MediaChangeLogo,
        //MediaChangeValid,

        MessageAdd = 510,
        MessageUpdate,
        MessageGet,
        MessageDelete,
        MessageList,
        MessageCount,
        //MessageGetValidItem,
        //MessageChangePhoto,
        //MessageChangeLogo,
        //MessageChangeValid,

        OffHoursAdd = 560,
        OffHoursUpdate,
        OffHoursGet,
        OffHoursDelete,
        OffHoursList,
        OffHoursCount,
        //OffHoursGetValidItem,
        //OffHoursChangePhoto,
        //OffHoursChangeLogo,
        //OffHoursChangeValid,

        PatientAdd = 610,
        PatientUpdate,
        PatientGet,
        PatientDelete,
        PatientList,
        PatientCount,
        PatientAllList,
        //PatientGetValidItem,
        //PatientChangePhoto,
        //PatientChangeLogo,
        //PatientChangeValid,

        PatientPrescriptionAdd = 660,
        PatientPrescriptionUpdate,
        PatientPrescriptionGet,
        PatientPrescriptionDelete,
        PatientPrescriptionList,
        PatientPrescriptionCount,
        PatientPrescriptionAllList,
        PatientPrescriptionAllDelete,
        PatientPrescriptionAllUpdate,
        //PatientPrescriptionGetValidItem,
        //PatientPrescriptionChangePhoto,
        //PatientPrescriptionChangeLogo,
        //PatientPrescriptionChangeValid,

        PatientPrescriptionMedicineAdd = 710,
        PatientPrescriptionMedicineUpdate,
        PatientPrescriptionMedicineGet,
        PatientPrescriptionMedicineDelete,
        PatientPrescriptionMedicineList,
        PatientPrescriptionMedicineCount,
        //PatientPrescriptionMedicineGetValidItem,
        //PatientPrescriptionMedicineChangePhoto,
        //PatientPrescriptionMedicineChangeLogo,
        //PatientPrescriptionMedicineChangeValid,

        PatientReportAdd = 760,
        PatientReportUpdate,
        PatientReportGet,
        PatientReportDelete,
        PatientReportList,
        PatientReportCount,
        PatientReportAllList,
        PatientReportAllDelete,
        PatientReportAllUpdate,
        //PatientReportGetValidItem,
        //PatientReportChangePhoto,
        //PatientReportChangeLogo,
        //PatientReportChangeValid,

        PatientTreatmentAdd = 810,
        PatientTreatmentUpdate,
        PatientTreatmentGet,
        PatientTreatmentDelete,
        PatientTreatmentList,
        PatientTreatmentCount,
        //PatientTreatmentGetValidItem,
        //PatientTreatmentChangePhoto,
        //PatientTreatmentChangeLogo,
        //PatientTreatmentChangeValid,

        PatientTreatmentServicesAdd = 860,
        PatientTreatmentServicesUpdate,
        PatientTreatmentServicesGet,
        PatientTreatmentServicesDelete,
        PatientTreatmentServicesList,
        PatientTreatmentServicesCount,
        //PatientTreatmentServicesGetValidItem,
        //PatientTreatmentServicesChangePhoto,
        //PatientTreatmentServicesChangeLogo,
        //PatientTreatmentServicesChangeValid,

        ReviewAdd = 910,
        ReviewUpdate,
        ReviewGet,
        ReviewDelete,
        ReviewList,
        ReviewCount,
        //ReviewGetValidItem,
        //ReviewChangePhoto,
        //ReviewChangeLogo,
        //ReviewChangeValid,

        RoleMethodAdd = 960,
        RoleMethodUpdate,
        RoleMethodGet,
        RoleMethodDelete,
        RoleMethodList,
        RoleMethodCount,
        RoleMethodAllList,
        //RoleMethodGetValidItem,
        //RoleMethodChangePhoto,
        //RoleMethodChangeLogo,
        //RoleMethodChangeValid,

        ServiceAdd = 1010,
        ServiceUpdate,
        ServiceGet,
        ServiceDelete,
        ServiceList,
        ServiceCount,
        //ServiceGetValidItem,
        //ServiceChangePhoto,
        ServiceChangeLogo,
        //ServiceChangeValid,

        SessionAdd = 1060,
        SessionUpdate,
        SessionGet,
        SessionDelete,
        SessionList,
        SessionCount,
        //SessionGetValidItem,
        //SessionChangePhoto,
        //SessionChangeLogo,
        //SessionChangeValid,

        SystemSettingAdd = 1110,
        SystemSettingUpdate,
        SystemSettingGet,
        SystemSettingDelete,
        SystemSettingList,
        SystemSettingCount,
        //SystemSettingGetValidItem,
        //SystemSettingChangePhoto,
        //SystemSettingChangeLogo,
        //SystemSettingChangeValid,

        UserAdd = 1160,
        UserUpdate,
        UserGet,
        UserDelete,
        UserList,
        UserCount,
        //UserGetValidItem,
        UserChangePhoto,
        //UserChangeLogo,
        //UserChangeValid,

        UserRoleAdd = 1210,
        UserRoleUpdate,
        UserRoleGet,
        UserRoleDelete,
        UserRoleList,
        UserRoleCount,
        //UserRoleGetValidItem,
        //UserRoleChangePhoto,
        //UserRoleChangeLogo,
        //UserRoleChangeValid,

        WorkingHourAdd = 1260,
        WorkingHourUpdate,
        WorkingHourGet,
        WorkingHourDelete,
        WorkingHourList,
        WorkingHourCount,
        //WorkingHourGetValidItem,
        //WorkingHourChangePhoto,
        //WorkingHourChangeLogo,
        //WorkingHourChangeValid,


        AppointmentRequestAdd = 1310,
        AppointmentRequestUpdate,
        AppointmentRequestGet,
        AppointmentRequestDelete,
        AppointmentRequestList,
        AppointmentRequestCount,
        AppointmentRequestAllList,
        AppointmentRequestAccept,
        AppointmentRequestReject,
        //AppointmentRequestGetValidItem,
        //AppointmentRequestChangePhoto,
        //AppointmentRequestChangeLogo,
        //AppointmentRequestChangeValid,


    }
}
