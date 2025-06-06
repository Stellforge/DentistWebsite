﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    public class SessionDto : DtoBase
    {
        public long UserId { get; set; }
        public string Key { get; set; }
        public string IpAddress { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public EDeviceType DeviceType { get; set; }

    }
}
