
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


    //interface IPostCommentRepository
    //{
    //    IEnumerable<PostComment> GetPostComments(int postId);
    //}
    public class PostCommentRepository : BaseRepository<PostComment>//,IPostCommentRepository
    {
        public PostCommentRepository(IceCreamBlogContext context) : base(context)
        {

        }

        //public    PostComment get(object ID)
        //{

        //    return this.GetByID(ID);
                
        //}

        //public new Task<IEnumerable<BlogPost>> GetAsync(Expression<Func<BlogPost, bool>> filter = null, Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null, string includeProperties = "")
        //{
        //    //TODO: IncludeImagePrefix dirty
        //    return new Task<IEnumerable<BlogPost>>(() =>
        //    {
        //        return this.GetAsync(filter, orderBy, includeProperties).Result
        //            .IncludeImagePrefix(Constants.BLOGPOST_IMAGE_PREFIX);
        //    });
        //}

        //TODO: IncludeImagePrefix dirty
        //public new IEnumerable<PostComment> Get(Expression<Func<PostComment, bool>> filter = null, Func<IQueryable<PostComment>, IOrderedQueryable<PostComment>> orderBy = null, string includeProperties = "")
        //{
        //    return base.Get(filter, orderBy, includeProperties);
                 
        //}

        //public IEnumerable<PostComment> GetPostComments(int postId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
