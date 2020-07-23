using SP_ASPNET_1.BusinessLogic;
using SP_ASPNET_1.DbFiles.UnitsOfWork;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_ASPNET_1.DbFiles.Operations
{
    public interface IBlogPostOperations {

           Task<BlogIndexViewModel> GetBlogIndexViewModelAsync(int page, int pageSize);
        BlogIndexViewModel GetBlogIndexViewModel();
        BlogPost GetBlogPostByIdD(int id);

        BlogSinglePostViewModel GetBlogPostByIdFull(int id);
          BlogSinglePostViewModel GetLatestBlogPost();
        BlogSinglePostViewModel GetRandomBlogPost();
        void Create(BlogPost blogPost);
        void Update(BlogPost blogPost);
        void Delete(int id);
          int LikePost(PostLike like);

        int UnlikePost(int userid, int PostId);

       int CountPostsLikesPerAuther(string autherId);
    }
    public class BlogPostOperations: IBlogPostOperations
    {
        public BlogPostOperations()
        {

        }
        private IUnitOfWork _unitOfWork;//= new UnitOfWork();
        public BlogPostOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BlogIndexViewModel> GetBlogIndexViewModelAsync(int page, int pageSize)
        {
            PagedResult<BlogPost> blogPosts = (await _unitOfWork.BlogPostSchoolRepository.GetAsync(null, b => b.OrderByDescending(d => d.DateTime), "Comments.Author,Likes", page ,pageSize));

            return new BlogIndexViewModel()
            {
                BlogPosts = blogPosts,
                BlogPost = blogPosts.Results.FirstOrDefault()
            };
        }

        public BlogIndexViewModel GetBlogIndexViewModel()
        {
            List<BlogPost> blogPosts = _unitOfWork.BlogPostSchoolRepository
                .Get(null, b => b.OrderByDescending(d => d.DateTime), "Author").ToList();
            if (!blogPosts.Any())
            {
                return new BlogIndexViewModel();
            }
            //return new BlogIndexViewModel()
            //{
            //    BlogPosts = blogPosts.GetRange(1, blogPosts.Count - 1),
            //    BlogPost = blogPosts.Take(1).FirstOrDefault()
            //};
            return null;
        }

        public BlogPost GetBlogPostByIdD(int id)
        {
            return _unitOfWork.BlogPostSchoolRepository.GetByID(id);
        }

        public BlogSinglePostViewModel GetBlogPostByIdFull(int id)
        {
            return _unitOfWork.BlogPostSchoolRepository.Get(filter: x => x.BlogPostID == id,
                    orderBy: null,
                    includeProperties: "Author,Comments.Author,Likes")
                .FirstOrDefault()
                .ToBlogSinglePostViewModel();
        }

        public BlogSinglePostViewModel GetLatestBlogPost()
        {
            return _unitOfWork.BlogPostSchoolRepository.Get(filter: null,
                    orderBy: x => x.OrderByDescending(entity => entity.DateTime),
                    includeProperties: "Author")
                .Select(StaticHelpers.ToBlogSinglePostViewModel)
                .FirstOrDefault();
        }


        public BlogSinglePostViewModel GetRandomBlogPost()
        {
            //TODO: Investigate
            List<BlogPost> posts = _unitOfWork.BlogPostSchoolRepository.Get(null,
                    x => x.OrderByDescending(entity => entity.DateTime),
                    "Author")
                .ToList();

            if(posts.Count is 0)
            {
                return null;
            }

            Random rnd = new Random();
            
            var randomPost = posts[rnd.Next(posts.Count)];
            return randomPost.ToBlogSinglePostViewModel();
        }

        public void Create(BlogPost blogPost)
        {
            try
            {
                this._unitOfWork.BlogPostSchoolRepository.Insert(blogPost);
                this._unitOfWork.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                BlogPost post = this.GetBlogPostByIdD(id);
                this._unitOfWork.BlogPostSchoolRepository.Remove(post);
                this._unitOfWork.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void Update(BlogPost blogPost)
        {
            _unitOfWork.BlogPostSchoolRepository.Update(blogPost);
            _unitOfWork.Save();
        }

        public int LikePost(PostLike like) {
           var result= _unitOfWork.BlogPostSchoolRepository.LikePost(like);
            _unitOfWork.Save();
            return _unitOfWork.BlogPostSchoolRepository.GetLikeCount(result.PostId);
        
        }
        public int UnlikePost(int userId, int postId) { 
              
       var result= _unitOfWork.BlogPostSchoolRepository.UnLikePost(userId, postId);
            _unitOfWork.Save();
            return _unitOfWork.BlogPostSchoolRepository.GetLikeCount(result.PostId);

        }

        public int CountPostsLikesPerAuther(string autherId)
        {
            return _unitOfWork.BlogPostSchoolRepository.CountPostsLikesPerAuther(autherId);
        }
    }
}