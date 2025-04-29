using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class BlogCategoryFilter:FilterBase
    {
        public string? Name { get; set; }
        public string? Explanation { get; set; }
    }
}
