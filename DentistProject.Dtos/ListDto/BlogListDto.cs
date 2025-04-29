using DentistProject.Dtos.Abstract;
using DentistProject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class BlogListDto:DtoBase
    {
        public long CategoryId { get; set; }
        public long PhotoId { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool OnAir {get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public string Keyword { get; set; }
        public long UserId { get; set; }

        
        public BlogCategoryListDto Category { get; set; }


        
        public UserListDto User { get; set; }
        
        
        public MediaListDto Photo { get; set; }



    }
}
