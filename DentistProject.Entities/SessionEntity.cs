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
    [Table("Session")]
    public class SessionEntity:EntityBase
    {
        public long UserId { get; set; }
        public string Key { get; set; }
        public string IpAddress { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public EDeviceType DeviceType { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }


    }
}
