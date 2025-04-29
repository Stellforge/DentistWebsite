using DentistProject.Core.DataAccess.EntityFramework;
using DentistProject.DataAccess.Abstract;
using DentistProject.DataAccess.EntityFramework;
using DentistProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.DataAccess.Repository.EntityFramework
{
    public class EfAppointmentRequestRepository:EfGenericRepositoryBase<AppointmentRequestEntity,DatabaseContext>,IAppointmentRequestRepository
    {
        protected override IQueryable<AppointmentRequestEntity> BaseGetAll(DatabaseContext context)
        {
            return base.BaseGetAll(context).Include(x => x.Patient).ThenInclude(x => x.User).Include(x => x.Dentist).ThenInclude(x => x.User); ;
        }
    }
}

