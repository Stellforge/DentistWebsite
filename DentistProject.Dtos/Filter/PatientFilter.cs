using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Filters.Filter
{
    
    public class PatientFilter : FilterBase

    {
        public string? NameSurname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }



        public EGender? Gender { get; set; }
        public string? Search { get; set; }
        public string? IdentityNumber { get; set; }
        
        
      
    }
}
