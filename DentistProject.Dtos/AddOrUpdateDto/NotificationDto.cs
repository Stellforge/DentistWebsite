using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class NotificationDto
    {
        public string[] Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Dictionary<string,string> Values { get; set; }
    }
}
