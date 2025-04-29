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
    
    public class DentistSocialFilter:FilterBase
    {
        public long? DentistId { get; set; }
        public ESocialMediaType? SocialMediaType { get; set; }



        
        


    }
}
