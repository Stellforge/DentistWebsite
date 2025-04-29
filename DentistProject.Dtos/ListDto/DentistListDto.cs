using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class DentistListDto:DtoBase
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Explantion { get; set; }
        public long PhotoId { get; set; }
        public DateTime JobStartDate { get; set; }
        public string Experience { get; set; }
        public int Awards { get; set; }

        
        public UserListDto User { get; set; }

        
        public MediaListDto Photo { get; set; }
    }
}
