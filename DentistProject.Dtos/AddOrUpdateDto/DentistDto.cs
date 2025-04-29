using DentistProject.Dtos.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class DentistDto:DtoBase
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Explantion { get; set; }
        public DateTime JobStartDate { get; set; }
        public string  Phone { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public int Awards { get; set; }

        // Todo: FaceBook - Linkedin - Twitter - Instagram ekleme eklenecek 
        // Todo: Açık adres 2 eklenecek  


        public string Facebook {  get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }

        public string Adress2 { get; set; }    


        public string Experience { get; set; }

        public UserDto User { get; set; }
        
        public IFormFile? Photo { get; set; }

        
        
    }
}
