using DentistProject.Dtos.Abstract;
using DentistProject.Entities.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Dtos.AddOrUpdateDto
{
    
    public class UserDto:DtoBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Todo : Ülkeler eklenecek
        // Todo : İL ŞEHİR EKLENECEK
        // Todo : İLçe EKLENECEK

        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public EGender Gender { get; set; }


        public ERoleType Role { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


        public IFormFile? ProfilePhoto { get; set; }

    }
}
