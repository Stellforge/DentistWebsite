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
    public class EfUserRepository:EfGenericRepositoryBase<UserEntity,DatabaseContext>,IUserRepository
    {
        protected override IQueryable<UserEntity> BaseGetAll(DatabaseContext context)
        {
            return base.BaseGetAll(context).Include(x => x.ProfilePhoto);
        }
    }
}

