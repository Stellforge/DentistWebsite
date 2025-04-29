using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class PatientDto : DtoBase

    {
        public UserDto User { get; set; }
        public string IdentityNumber { get; set; }

        public string Phone2 { get; set; }

        // Todo: phone2 eklenece 

      
        
        
      
    }
}
