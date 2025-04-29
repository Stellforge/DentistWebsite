using AutoMapper;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.ListDto;
using DentistProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Mapping.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AboutDto, AboutEntity>().ReverseMap();
            CreateMap<AboutEntity, AboutListDto>().ReverseMap();
            //CreateMap<AboutDto,AboutListDto>().ReverseMap();


            CreateMap<AppointmentDto, AppointmentEntity>().ReverseMap();
            CreateMap<AppointmentEntity, AppointmentListDto>().ReverseMap();
            //CreateMap<AppointmentDto,AppointmentListDto>().ReverseMap();


            CreateMap<AppointmentRequestDto, AppointmentRequestEntity>().ReverseMap();
            CreateMap<AppointmentRequestEntity, AppointmentRequestListDto>().ReverseMap();
            //CreateMap<AppointmentRequestDto,AppointmentRequestListDto>().ReverseMap();



            CreateMap<BlogCategoryDto, BlogCategoryEntity>().ReverseMap();
            CreateMap<BlogCategoryEntity, BlogCategoryListDto>().ReverseMap();
            //CreateMap<BlogCategoryDto,BlogCategoryListDto>().ReverseMap();


            CreateMap<BlogDto, BlogEntity>().ForMember(x => x.Photo, opt => opt.Ignore()).ReverseMap();
            CreateMap<BlogEntity, BlogListDto>().ReverseMap();
            CreateMap<BlogDto, BlogListDto>().ForMember(x => x.Photo, opt => opt.Ignore());
            CreateMap<BlogListDto, BlogDto>().ForMember(x => x.Photo, opt => opt.Ignore());


            CreateMap<ContactDto, ContactEntity>().ReverseMap();
            CreateMap<ContactEntity, ContactListDto>().ReverseMap();
            //CreateMap<ContactDto,ContactListDto>().ReverseMap();


            CreateMap<DentistDto, DentistEntity>().ForMember(x => x.Photo, x => x.Ignore()).ReverseMap();
            CreateMap<DentistEntity, DentistListDto>().ReverseMap();
            CreateMap<DentistDto, DentistListDto>().ForMember(x => x.Photo, x => x.Ignore());
            CreateMap<DentistListDto, DentistDto>().ForMember(x => x.Photo, x => x.Ignore());


            CreateMap<DentistSocialDto, DentistSocialEntity>().ReverseMap();
            CreateMap<DentistSocialEntity, DentistSocialListDto>().ReverseMap();
            //CreateMap<DentistSocialDto,DentistSocialListDto>().ReverseMap();


            CreateMap<IdentityDto, IdentityEntity>().ReverseMap();
            CreateMap<IdentityEntity, IdentityListDto>().ReverseMap();
            //CreateMap<IdentityDto,IdentityListDto>().ReverseMap();


            CreateMap<InvoiceDto, InvoiceEntity>().ReverseMap();
            CreateMap<InvoiceEntity, InvoiceListDto>().ReverseMap();
            //CreateMap<InvoiceDto,InvoiceListDto>().ReverseMap();


            CreateMap<MediaDto, MediaEntity>().ReverseMap();
            CreateMap<MediaEntity, MediaListDto>().ReverseMap();
            //CreateMap<MediaDto,MediaListDto>().ReverseMap();


            CreateMap<MessageDto, MessageEntity>().ReverseMap();
            CreateMap<MessageEntity, MessageListDto>().ReverseMap();
            //CreateMap<MessageDto,MessageListDto>().ReverseMap();


            CreateMap<OffHoursDto, OffHoursEntity>().ReverseMap();
            CreateMap<OffHoursEntity, OffHoursListDto>().ReverseMap();
            //CreateMap<OffHoursDto,OffHoursListDto>().ReverseMap();


            CreateMap<PatientDto, PatientEntity>().ReverseMap();
            CreateMap<PatientEntity, PatientListDto>().ReverseMap();
            CreateMap<PatientDto,PatientListDto>().ReverseMap();


            CreateMap<PatientPrescriptionDto, PatientPrescriptionEntity>().ReverseMap();
            CreateMap<PatientPrescriptionEntity, PatientPrescriptionListDto>().ReverseMap();
            //CreateMap<PatientPrescriptionDto,PatientPrescriptionListDto>().ReverseMap();


            CreateMap<PatientPrescriptionMedicineDto, PatientPrescriptionMedicineEntity>().ReverseMap();
            CreateMap<PatientPrescriptionMedicineEntity, PatientPrescriptionMedicineListDto>().ReverseMap();
            //CreateMap<PatientPrescriptionMedicineDto,PatientPrescriptionMedicineListDto>().ReverseMap();


            CreateMap<PatientReportDto, PatientReportEntity>().ForMember(x => x.File, opt => opt.Ignore()).ReverseMap();
            CreateMap<PatientReportEntity, PatientReportListDto>().ReverseMap();
            //CreateMap<PatientReportDto,PatientReportListDto>().ReverseMap();


            CreateMap<PatientTreatmentDto, PatientTreatmentEntity>().ReverseMap();
            CreateMap<PatientTreatmentEntity, PatientTreatmentListDto>().ReverseMap();
            //CreateMap<PatientTreatmentDto,PatientTreatmentListDto>().ReverseMap();


            CreateMap<PatientTreatmentServicesDto, PatientTreatmentServicesEntity>().ReverseMap();
            CreateMap<PatientTreatmentServicesEntity, PatientTreatmentServicesListDto>().ReverseMap();
            //CreateMap<PatientTreatmentServicesDto,PatientTreatmentServicesListDto>().ReverseMap();


            CreateMap<ReviewDto, ReviewEntity>().ReverseMap();
            CreateMap<ReviewEntity, ReviewListDto>().ReverseMap();
            //CreateMap<ReviewDto,ReviewListDto>().ReverseMap();


            CreateMap<RoleMethodDto, RoleMethodEntity>().ReverseMap();
            CreateMap<RoleMethodEntity, RoleMethodListDto>().ReverseMap();
            //CreateMap<RoleMethodDto,RoleMethodListDto>().ReverseMap();


            CreateMap<ServiceDto, ServiceEntity>().ForMember(x => x.Logo, opt => opt.Ignore()).ReverseMap();
            CreateMap<ServiceEntity, ServiceListDto>().ReverseMap();
            //CreateMap<ServiceDto,ServiceListDto>().ReverseMap();


            CreateMap<SessionDto, SessionEntity>().ReverseMap();
            CreateMap<SessionEntity, SessionListDto>().ReverseMap();
            CreateMap<SessionDto,SessionListDto>().ReverseMap();


            CreateMap<SystemSettingDto, SystemSettingEntity>().ReverseMap();
            CreateMap<SystemSettingEntity, SystemSettingListDto>().ReverseMap();
            //CreateMap<SystemSettingDto,SystemSettingListDto>().ReverseMap();


            CreateMap<UserDto, UserEntity>().ForMember(x => x.ProfilePhoto, opt => opt.Ignore()).ReverseMap();
            CreateMap<UserEntity, UserListDto>().ReverseMap();
            CreateMap<UserDto,UserListDto>().ForMember(x=>x.ProfilePhoto,opt=>opt.Ignore());
            CreateMap<UserListDto,UserDto>().ForMember(x=>x.ProfilePhoto,opt=>opt.Ignore());


            CreateMap<UserRoleDto, UserRoleEntity>().ReverseMap();
            CreateMap<UserRoleEntity, UserRoleListDto>().ReverseMap();
            //CreateMap<UserRoleDto,UserRoleListDto>().ReverseMap();


            CreateMap<WorkingHourDto, WorkingHourEntity>().ReverseMap();
            CreateMap<WorkingHourEntity, WorkingHourListDto>().ReverseMap();
            //CreateMap<WorkingHourDto,WorkingHourListDto>().ReverseMap();




        }
    }
}
