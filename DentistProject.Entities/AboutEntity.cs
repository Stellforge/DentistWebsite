using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("About")]
    public class AboutEntity:EntityBase
    {
        public bool IsValid { get; set; }
        public string OurWork { get; set; }
        public string MissionStatement { get; set; }
        public string Target { get; set; }
        public string Explanation { get; set; }
    }
}
