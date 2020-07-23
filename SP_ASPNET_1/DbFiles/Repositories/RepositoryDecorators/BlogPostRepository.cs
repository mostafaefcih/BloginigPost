
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SP_ASPNET_1.BusinessLogic;
using SP_ASPNET_1.DbFiles.Contexts;
using SP_ASPNET_1.DbFiles.Repositories;
using SP_ASPNET_1.Models;

namespace SP_ASPNET_1.DbFiles.Operations
{
    public interface IBlogPostRepository: IRepository<BlogPost>
    {
        PostLike LikePost(PostLike like);
        PostLike UnLikePost(int userId,int postId);
        int GetLikeCount(int postId);
        //  this method to count all likes on auther's posts
        int CountPostsLikesPerAuther(string autherId);
    }
    public class BlogPostRepository : BaseRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(IceCreamBlogContext context) : base(context)
        {

        }

        public new BlogPost GetByID(object ID)
        {
            //TODO: IncludeImagePrefix dirty
            return (new BlogPost[] { this.GetByID(ID) }).IncludeImagePrefix(Constants.BLOGPOST_IMAGE_PREFIX)
                .FirstOrDefault();
        }

        public new Task<IEnumerable<BlogPost>> GetAsync(Expression<Func<BlogPost, bool>> filter = null, Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null, string includeProperties = "")
        {
            //TODO: IncludeImagePrefix dirty
            return new Task<IEnumerable<BlogPost>>(() =>
            {
                return this.GetAsync(filter, orderBy, includeProperties).Result;
                    //.IncludeImagePrefix(Constants.BLOGPOST_IMAGE_PREFIX);
            });
        }

        //TODO: IncludeImagePrefix dirty
        public new IEnumerable<BlogPost> Get(Expression<Func<BlogPost, bool>> filter = null, Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null, string includeProperties = "")
        {
            return base.Get(filter, orderBy, includeProperties);
                //.IncludeImagePrefix(Constants.BLOGPOST_IMAGE_PREFIX);
        }

        public PostLike LikePost(PostLike like)
        {
            return _context.Likes.Add(like);
        }

        public PostLike UnLikePost(int userId,int postId)
        {
            return _context.Likes.Remove(_context.Likes.Where(l => l.PostId == postId && l.AuthorId == Convert.ToString(userId)).FirstOrDefault());
        }

        public int GetLikeCount(int postId)
        {
            return _context.Likes.Count(l => l.PostId == postId);
        }


        public int CountPostsLikesPerAuther(string autherId)
        {
          var postsId=  _context.BlogPosts.Where(c => c.Author.Id == autherId).Select(c=>c.BlogPostID);
            return _context.Likes.Where(c => postsId.Contains(c.PostId)).Count();
        }
    }
}
