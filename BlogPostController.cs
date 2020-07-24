using Microsoft.AspNet.Identity;
using SP_ASPNET_1.DbFiles.Operations;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SP_ASPNET_1.Controllers
{
    [RoutePrefix("Blog")]
    public class BlogPostController : Controller
    {
        private readonly IBlogPostOperations _blogPostOperations;//= new BlogPostOperations();
        private static Dictionary<string, string> authers = new Dictionary<string, string>();

        public BlogPostController(IBlogPostOperations blogPostOperations)
        {
            _blogPostOperations = blogPostOperations;
        }
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index(int page=1 ,int pageSize=10)
        {
            //return this.View();
            BlogIndexViewModel result = await this._blogPostOperations.GetBlogIndexViewModelAsync(page ,pageSize);
            foreach (var item in result.BlogPosts.Results)
            {
                if (!authers.ContainsKey(item.AuthorID))
                {
                    item.Author.AutherLikes = _blogPostOperations.CountPostsLikesPerAuther(item.AuthorID);
                    authers.Add(item.AuthorID, item.Author.AutherLikes.ToString());
                }
                else
                {
                    var count = authers.FirstOrDefault(x=>x.Key== item.AuthorID).Value;
                    item.Author.AutherLikes =Convert.ToInt32(count);

                }

            }
            ViewBag.Title = "Blog";
            return this.View(result);   
        }


        [Route("Detail/{id:int?}")]
        [HttpGet]
        public ActionResult SinglePost(int? id)
        {
            ViewBag.Title = "single post";

            
            BlogSinglePostViewModel modelView;

            if (id == null)
            {
                modelView = this._blogPostOperations.GetLatestBlogPost();
            }
            else
            {
                modelView = this._blogPostOperations.GetBlogPostByIdFull((int)id);
            }

            return View(modelView);
        }

        [Route("Detail/Random")]
        [HttpGet]
        public ActionResult RandomPost()
        {
            ViewBag.Title = "Random post";

            var viewModel = this._blogPostOperations.GetRandomBlogPost();

            return View(viewModel);
        }

        [Route("LatestPost")]
        [HttpGet]
        public ActionResult LatestPost()
        {
            var viewModel = this._blogPostOperations.GetLatestBlogPost();

            return this.PartialView("~/Views/BlogPost/_BlogPostRecentPartialView.cshtml", viewModel);
        }

        [Route("Create")]
        [HttpPost]
        [Authorize(Roles = "AUTHOR")]
        public ActionResult Create(BlogPost blogPost)
        {
            try
            {
                blogPost.AuthorID=  User.Identity.GetUserId();
                blogPost.DateTime = DateTime.Now;
                this._blogPostOperations.Create(blogPost);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Route("Edit/{id:int?}")]
        [HttpGet]
        [Authorize(Roles = "AUTHOR")]
        public ActionResult EditBlogPost(int id)
        {
            BlogPost blogPost;

            blogPost = this._blogPostOperations.GetBlogPostByIdD((int)id);

            return View(blogPost);
        }

        [Route("Edit/{id:int}")]
        [HttpPost]
        [Authorize(Roles = "AUTHOR")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                // TODO: Return to detail
                throw new NotImplementedException("Editing blog post is not implemented");
            }
            catch
            {
                return View();
            }
        }

        [Route("Delete/{id:int}")]
        [HttpGet]
        [Authorize(Roles ="AUTHOR")]

        public ActionResult Delete(int id)
        {
            try
            {
                //if(id==null)
                this._blogPostOperations.Delete(id);

                //CHECK: should return to blogs
                return RedirectToAction("Index");
            }
            catch
            {
                return this.HttpNotFound();
            }
        }
        [Route("Like/{postId:int}")]
        [HttpGet]
        [Authorize]
        public JsonResult LikePost(int postId)
        {
            var PostLike = new PostLike()
            {
                DateTime = DateTime.Now,
                AuthorId = User.Identity.GetUserId(),
                PostId = postId
            };
         var counter=  _blogPostOperations.LikePost(PostLike);
            //return counter.ToString();
            return Json(counter, JsonRequestBehavior.AllowGet);
        }
        [Route("unLike/{postId:int}")]
        [HttpGet]
        [Authorize]

        public ActionResult UnLikePost(int postId)
        {
            var counter = _blogPostOperations.UnlikePost(1, postId);
            
            return View();
        }

        int CountPostsLikesPerAuther( ) {
            var autherId = User.Identity.GetUserId();
            return _blogPostOperations.CountPostsLikesPerAuther(autherId);
        }

    }
}
