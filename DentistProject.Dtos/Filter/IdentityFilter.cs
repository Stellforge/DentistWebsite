using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class IdentityFilter:FilterBase
    {
        public long? UserId { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? ExpiryDate { get; set; }


        
        
    }
}
