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
    
    public class ServiceDto:DtoBase
    {
        public string Title { get; set; }
        public IFormFile  Logo { get; set; }
        public string Explanation { get; set; }
        public Decimal  Price { get; set; }

        
        

    }
}
