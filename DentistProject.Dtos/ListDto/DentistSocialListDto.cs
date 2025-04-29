using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class DentistSocialListDto:DtoBase
    {
        public long DentistId { get; set; }
        public ESocialMediaType SocialMediaType { get; set; }

        public string Link { get; set; }
        public string Title { get; set; }

        
        public DentistListDto Dentist { get; set; }


    }
}
