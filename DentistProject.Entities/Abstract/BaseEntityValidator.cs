using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities.Abstract
{
    public class BaseEntityValidator<T> : AbstractValidator<T>
        where T : EntityBase, new()
    {
        public BaseEntityValidator()
        {
            RuleFor(x => x.IsDeleted).NotNull();
            RuleFor(x => x.CreateTime).NotEmpty().NotNull();
        }
    }
}
