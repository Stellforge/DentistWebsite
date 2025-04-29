using DentistProject.Business;
using DentistProject.Business.Abstract;
using DentistProject.Business.Mapping.AutoMapper;
using DentistProject.Core.DataAccess;
using DentistProject.DataAccess.Repository.EntityFramework;
using DentistProject.Entities;
using DentistProject.Entities.Abstract;
using DentistProject.Entities.Validator;
using Microsoft.AspNetCore.Http.Features;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IEntityRepository<AboutEntity>, EfAboutRepository>();
builder.Services.AddScoped<BaseEntityValidator<AboutEntity>, AboutValidator>();



builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IEntityRepository<AppointmentEntity>, EfAppointmentRepository>();
builder.Services.AddScoped<BaseEntityValidator<AppointmentEntity>, AppointmentValidator>();


builder.Services.AddScoped<IAppointmentRequestService, AppointmentRequestManager>();
builder.Services.AddScoped<IEntityRepository<AppointmentRequestEntity>, EfAppointmentRequestRepository>();
builder.Services.AddScoped<BaseEntityValidator<AppointmentRequestEntity>, AppointmentRequestValidator>();



builder.Services.AddScoped<IBlogCategoryService, BlogCategoryManager>();
builder.Services.AddScoped<IEntityRepository<BlogCategoryEntity>, EfBlogCategoryRepository>();
builder.Services.AddScoped<BaseEntityValidator<BlogCategoryEntity>, BlogCategoryValidator>();



builder.Services.AddScoped<IBlogService, BlogManager>();
builder.Services.AddScoped<IEntityRepository<BlogEntity>, EfBlogRepository>();
builder.Services.AddScoped<BaseEntityValidator<BlogEntity>, BlogValidator>();



builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IEntityRepository<ContactEntity>, EfContactRepository>();
builder.Services.AddScoped<BaseEntityValidator<ContactEntity>, ContactValidator>();



builder.Services.AddScoped<IDentistService, DentistManager>();
builder.Services.AddScoped<IEntityRepository<DentistEntity>, EfDentistRepository>();
builder.Services.AddScoped<BaseEntityValidator<DentistEntity>, DentistValidator>();



builder.Services.AddScoped<IDentistSocialService, DentistSocialManager>();
builder.Services.AddScoped<IEntityRepository<DentistSocialEntity>, EfDentistSocialRepository>();
builder.Services.AddScoped<BaseEntityValidator<DentistSocialEntity>, DentistSocialValidator>();



builder.Services.AddScoped<IIdentityService, IdentityManager>();
builder.Services.AddScoped<IEntityRepository<IdentityEntity>, EfIdentityRepository>();
builder.Services.AddScoped<BaseEntityValidator<IdentityEntity>, IdentityValidator>();



builder.Services.AddScoped<IInvoiceService, InvoiceManager>();
builder.Services.AddScoped<IEntityRepository<InvoiceEntity>, EfInvoiceRepository>();
builder.Services.AddScoped<BaseEntityValidator<InvoiceEntity>, InvoiceValidator>();



builder.Services.AddScoped<IMediaService, MediaManager>();
builder.Services.AddScoped<IEntityRepository<MediaEntity>, EfMediaRepository>();
builder.Services.AddScoped<BaseEntityValidator<MediaEntity>, MediaValidator>();



builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<IEntityRepository<MessageEntity>, EfMessageRepository>();
builder.Services.AddScoped<BaseEntityValidator<MessageEntity>, MessageValidator>();



builder.Services.AddScoped<IOffHoursService, OffHoursManager>();
builder.Services.AddScoped<IEntityRepository<OffHoursEntity>, EfOffHoursRepository>();
builder.Services.AddScoped<BaseEntityValidator<OffHoursEntity>, OffHoursValidator>();



builder.Services.AddScoped<IPatientService, PatientManager>();
builder.Services.AddScoped<IEntityRepository<PatientEntity>, EfPatientRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientEntity>, PatientValidator>();



builder.Services.AddScoped<IPatientPrescriptionService, PatientPrescriptionManager>();
builder.Services.AddScoped<IEntityRepository<PatientPrescriptionEntity>, EfPatientPrescriptionRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientPrescriptionEntity>, PatientPrescriptionValidator>();



builder.Services.AddScoped<IPatientPrescriptionMedicineService, PatientPrescriptionMedicineManager>();
builder.Services.AddScoped<IEntityRepository<PatientPrescriptionMedicineEntity>, EfPatientPrescriptionMedicineRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientPrescriptionMedicineEntity>, PatientPrescriptionMedicineValidator>();



builder.Services.AddScoped<IPatientReportService, PatientReportManager>();
builder.Services.AddScoped<IEntityRepository<PatientReportEntity>, EfPatientReportRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientReportEntity>, PatientReportValidator>();



builder.Services.AddScoped<IPatientTreatmentService, PatientTreatmentManager>();
builder.Services.AddScoped<IEntityRepository<PatientTreatmentEntity>, EfPatientTreatmentRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientTreatmentEntity>, PatientTreatmentValidator>();



builder.Services.AddScoped<IPatientTreatmentServicesService, PatientTreatmentServicesManager>();
builder.Services.AddScoped<IEntityRepository<PatientTreatmentServicesEntity>, EfPatientTreatmentServicesRepository>();
builder.Services.AddScoped<BaseEntityValidator<PatientTreatmentServicesEntity>, PatientTreatmentServicesValidator>();



builder.Services.AddScoped<IReviewService, ReviewManager>();
builder.Services.AddScoped<IEntityRepository<ReviewEntity>, EfReviewRepository>();
builder.Services.AddScoped<BaseEntityValidator<ReviewEntity>, ReviewValidator>();



builder.Services.AddScoped<IRoleMethodService, RoleMethodManager>();
builder.Services.AddScoped<IEntityRepository<RoleMethodEntity>, EfRoleMethodRepository>();
builder.Services.AddScoped<BaseEntityValidator<RoleMethodEntity>, RoleMethodValidator>();



builder.Services.AddScoped<IServiceService, ServiceManager>();
builder.Services.AddScoped<IEntityRepository<ServiceEntity>, EfServiceRepository>();
builder.Services.AddScoped<BaseEntityValidator<ServiceEntity>, ServiceValidator>();



builder.Services.AddScoped<ISessionService, SessionManager>();
builder.Services.AddScoped<IEntityRepository<SessionEntity>, EfSessionRepository>();
builder.Services.AddScoped<BaseEntityValidator<SessionEntity>, SessionValidator>();



builder.Services.AddScoped<ISystemSettingService, SystemSettingManager>();
builder.Services.AddScoped<IEntityRepository<SystemSettingEntity>, EfSystemSettingRepository>();
builder.Services.AddScoped<BaseEntityValidator<SystemSettingEntity>, SystemSettingValidator>();



builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IEntityRepository<UserEntity>, EfUserRepository>();
builder.Services.AddScoped<BaseEntityValidator<UserEntity>, UserValidator>();



builder.Services.AddScoped<IUserRoleService, UserRoleManager>();
builder.Services.AddScoped<IEntityRepository<UserRoleEntity>, EfUserRoleRepository>();
builder.Services.AddScoped<BaseEntityValidator<UserRoleEntity>, UserRoleValidator>();



builder.Services.AddScoped<IWorkingHourService, WorkingHourManager>();
builder.Services.AddScoped<IEntityRepository<WorkingHourEntity>, EfWorkingHourRepository>();
builder.Services.AddScoped<BaseEntityValidator<WorkingHourEntity>, WorkingHourValidator>();



builder.Services.AddScoped<IAccountService,AccountManager>();

builder.Services.AddScoped<INotificationService, NotificationManager>();
builder.Services.AddScoped<IAccountService, AccountManager>();


//Toast
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});

//Form file size changed
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;

});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
