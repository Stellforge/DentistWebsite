using DentistProject.Entities.Abstract;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("DentistSocial")]
    public class DentistSocialEntity:EntityBase
    {
        public long DentistId { get; set; }
        public ESocialMediaType SocialMediaType { get; set; }

        public string Link { get; set; }
        public string Title { get; set; }

        [ForeignKey(nameof(DentistId))]
        public DentistEntity Dentist { get; set; }


    }
}
