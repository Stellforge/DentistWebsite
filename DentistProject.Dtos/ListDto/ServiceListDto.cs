using DentistProject.Dtos.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.ListDto
{
    
    public class ServiceListDto:DtoBase
    {
        public string Title { get; set; }
        public long  LogoId { get; set; }
        public string Explanation { get; set; }
        public Decimal  Price { get; set; }

        
        public MediaListDto Logo { get; set; }

    }
}
