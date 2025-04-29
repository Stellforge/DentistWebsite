using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
   public class ReviewDto:DtoBase
    {
        public string Review { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
        public long UserId { get; set; }

        
        
    }
}
