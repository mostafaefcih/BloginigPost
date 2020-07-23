using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SP_ASPNET_1.Models
{
    public class BlogPost
    {
        public int BlogPostID { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [ForeignKey(nameof(Author))]
        public string AuthorID { get; set; }
        [Required]

        public ApplicationUser Author { get; set; }
        public DateTime DateTime { get; set; }
        [Required]

        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<PostComment> Comments { get; set; }
        public ICollection<PostLike> Likes { get; set; }

    }
}