using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class PostLike
    {
        [Key]
        public int LikeId { get; set; }
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }


        public BlogPost Post { get; set; }
        public ApplicationUser Author { get; set; }
         public DateTime DateTime { get; set; }


    }
}