using DentistProject.Core.DataAccess.EntityFramework;
using DentistProject.DataAccess.Abstract;
using DentistProject.DataAccess.EntityFramework;
using DentistProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.DataAccess.Repository.EntityFramework
{
    public class EfMediaRepository:EfGenericRepositoryBase<MediaEntity,DatabaseContext>,IMediaRepository
    {
        protected override IQueryable<MediaEntity> BaseGetAll(DatabaseContext context)
        {
            return base.BaseGetAll(context);
        }
    }
}

