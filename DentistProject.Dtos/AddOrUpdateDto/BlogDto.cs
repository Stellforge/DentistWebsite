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
    
    public class BlogDto:DtoBase
    {
        public long CategoryId { get; set; }
        public IFormFile Photo { get; set; }
        public bool OnAir {get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public string Keyword { get; set; }
        public long UserId { get; set; }

        
        


        
        
        
        
        



    }
}
