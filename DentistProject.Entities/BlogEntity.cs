using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Entities
{
    [Table("Blog")]
    public class BlogEntity : EntityBase
    {
        public long CategoryId { get; set; }
        public long PhotoId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool OnAir { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public string Keyword { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public BlogCategoryEntity Category { get; set; }


        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        [ForeignKey(nameof(PhotoId))]
        public MediaEntity Photo { get; set; }



       

    }
}
