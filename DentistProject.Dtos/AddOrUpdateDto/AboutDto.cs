using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class AboutDto:DtoBase
    {
        public bool IsValid { get; set; }
        public string OurWork { get; set; }
        public string MissionStatement { get; set; }
        public string Target { get; set; }
        public string Explanation { get; set; }
    }
}
