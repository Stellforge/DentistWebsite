using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class IdentityListDto:DtoBase
    {
        public long UserId { get; set; }
        
        public DateTime? ExpiryDate { get; set; }


        
        public UserListDto User { get; set; }
    }
}
