using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class ContactListDto : DtoBase
    {
        public bool Validity { get; set; }
        public string Adress { get; set; }
        public string Name { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
      

        //LİNKLER
        public string Email { get; set; }
        public string FacebookLink { get; set; }
        public string XLink { get; set; }
        public string YoutubeLink { get; set; }
        public string InstagramLink { get; set; }
        public string GoogleMapLink { get; set; }


        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
