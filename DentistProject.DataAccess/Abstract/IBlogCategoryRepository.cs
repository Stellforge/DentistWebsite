using DentistProject.Core.DataAccess;
using DentistProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.DataAccess.Abstract
{
    public interface IBlogCategoryRepository:IEntityRepository<BlogCategoryEntity>
    {
    }
}

