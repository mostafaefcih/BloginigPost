using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class PostComment
    {
        [Key]
        public int CommentId { get; set; }

        [ForeignKey(nameof(Post))]
        [Required]

        public int PostId { get; set; }
        [ForeignKey(nameof(Author))]
        [Required]

        public string AuthorId { get; set; }
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }
        public ApplicationUser Author { get; set; }
        public BlogPost Post { get; set; }
        public DateTime DateTime { get; set; }


    }
}