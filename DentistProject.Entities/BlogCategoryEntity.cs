using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("BlogCategory")]
    public class BlogCategoryEntity:EntityBase
    {
        public string Name { get; set; }
        public string Explanation { get; set; }
    }
}
