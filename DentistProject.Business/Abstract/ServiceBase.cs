using AutoMapper;
using DentistProject.Core.DataAccess;
using DentistProject.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Abstract
{
    public abstract class ServiceBase<TEntity>
       where TEntity : EntityBase, new()
    {
        protected IEntityRepository<TEntity> Repository { get; private set; }

        protected IMapper Mapper { get; private set; }
        protected IHttpContextAccessor HttpContext { get; private set; }
        protected string IpAddress
        {
            get
            {
                return HttpContext.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        protected BaseEntityValidator<TEntity> Validator { get; private set; }

        protected ServiceBase(IEntityRepository<TEntity> repository, IMapper mapper, BaseEntityValidator<TEntity> validator, IHttpContextAccessor httpContext)
        {
            Repository = repository;
            Mapper = mapper;
            Validator = validator;
            HttpContext = httpContext;
        }
    }
}
