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
    [Table("User")]
    public class UserEntity : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }



        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public EGender Gender { get; set; }

        public long? ProfilePhotoId { get; set; }

        [ForeignKey(nameof(ProfilePhotoId))]
        public virtual MediaEntity ProfilePhoto { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }


    }
}
