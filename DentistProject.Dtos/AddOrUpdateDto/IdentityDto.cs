using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class IdentityDto:DtoBase
    {
        public long UserId { get; set; }
        public string Password { get; set; }

        
        public string ConfirmPassword { get; set; }
        public DateTime ExpiryDate { get; set; }


        
        
    }
}
