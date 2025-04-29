using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class DentistSocialAddDto:DtoBase
    {
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string XLink { get; set; }
        public string YoutubeLink { get; set; }
        public string WebsiteLink { get; set; }

    }
}
