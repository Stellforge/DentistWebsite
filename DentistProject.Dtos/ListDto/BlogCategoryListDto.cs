using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class BlogCategoryListDto:DtoBase
    {
        public string Name { get; set; }
        public string Explanation { get; set; }
    }
}
