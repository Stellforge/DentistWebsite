using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class AcceptAppoimentRequestDto
    {
        public DateTime InspactionDate { get; set; }
        public float InspactionTimeHour { get; set; }

        public string Info { get; set; }


    }
}
