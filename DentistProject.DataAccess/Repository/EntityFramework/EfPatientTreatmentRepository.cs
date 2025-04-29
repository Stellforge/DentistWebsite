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
    public class EfPatientTreatmentRepository:EfGenericRepositoryBase<PatientTreatmentEntity,DatabaseContext>,IPatientTreatmentRepository
    {
        protected override IQueryable<PatientTreatmentEntity> BaseGetAll(DatabaseContext context)
        {
            return base.BaseGetAll(context).Include(x => x.Patient).ThenInclude(x => x.User).Include(x => x.InterveningDentist);
        }
    }
}

