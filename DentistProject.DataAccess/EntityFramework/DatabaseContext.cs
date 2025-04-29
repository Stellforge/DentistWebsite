using DentistProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.DataAccess.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;Initial Catalog= DentistProjectDB;Integrated Security =True;Encrypt=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }



        public DbSet<AboutEntity> Abouts { get; set; }


        public DbSet<AppointmentEntity> Appointments { get; set; }


        public DbSet<AppointmentRequestEntity> AppointmentRequests { get; set; }


        public DbSet<BlogCategoryEntity> BlogCategorys { get; set; }


        public DbSet<BlogEntity> Blogs { get; set; }


        public DbSet<ContactEntity> Contacts { get; set; }


        public DbSet<DentistEntity> Dentists { get; set; }


        public DbSet<DentistSocialEntity> DentistSocials { get; set; }


        public DbSet<IdentityEntity> Identitys { get; set; }


        public DbSet<InvoiceEntity> Invoices { get; set; }


        public DbSet<MediaEntity> Medias { get; set; }


        public DbSet<MessageEntity> Messages { get; set; }


        public DbSet<OffHoursEntity> OffHours { get; set; }


        public DbSet<PatientEntity> Patients { get; set; }


        public DbSet<PatientPrescriptionEntity> PatientPrescriptions { get; set; }


        public DbSet<PatientPrescriptionMedicineEntity> PatientPrescriptionMedicines { get; set; }


        public DbSet<PatientReportEntity> PatientReports { get; set; }


        public DbSet<PatientTreatmentEntity> PatientTreatments { get; set; }


        public DbSet<PatientTreatmentServicesEntity> PatientTreatmentServices { get; set; }


        public DbSet<ReviewEntity> Reviews { get; set; }




        public DbSet<RoleMethodEntity> RoleMethods { get; set; }


        public DbSet<ServiceEntity> Services { get; set; }
        
        
        public DbSet<SessionEntity> Sessions { get; set; }
               

        public DbSet<SystemSettingEntity> SystemSettings { get; set; }


        public DbSet<UserEntity> Users { get; set; }


        public DbSet<UserRoleEntity> UserRoles { get; set; }


        public DbSet<WorkingHourEntity> WorkingHours { get; set; }




    }
}
