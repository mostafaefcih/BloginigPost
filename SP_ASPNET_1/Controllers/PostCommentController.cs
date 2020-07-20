using SP_ASPNET_1.DbFiles.Operations;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace SP_ASPNET_1.Controllers
{
    [RoutePrefix("Comment")]
    public class PostCommentController : Controller
    {
        private readonly IPostCommentOperations _PostCommentOperations;//= new PostCommentOperations();
        public PostCommentController(IPostCommentOperations PostCommentOperations)
        {
            _PostCommentOperations = PostCommentOperations;
        }
        [Route("")]
        [HttpGet]
        public ActionResult Index(int postId)
        {
            //return this.View();
            IEnumerable<PostComment> result = this._PostCommentOperations.GetCommentsPerPost(postId);
            return PartialView("_postComments",result);
        }


        //[Route("Detail/{id:int?}")]
        //[HttpGet]
        //public ActionResult Detail(int id)
        //{
            
            
        //    PostComment comment;


        //    comment = _PostCommentOperations.GetCommentById(id);
          

        //    return View(comment);
        //}


        [Route("Create")]
        [HttpPost]
        public ActionResult Create(int postId, string CommentBody)
        {
            try
            {
                
                PostComment comment = new PostComment() { 
                AuthorId=1, // logged in user
                Content=CommentBody,
                DateTime=DateTime.Now,
                PostId=  postId,
                   Title=CommentBody
                };

                 _PostCommentOperations.Create(comment);

                return RedirectToAction("Index",new { postId = postId});
            }
            catch
            {
                return View();
            }
        }

        [Route("Edit/{id:int?}")]
        [HttpGet]
        public ActionResult EditPostComment(int id)
        {
            PostComment PostComment;

            PostComment = this._PostCommentOperations.GetCommentById((int)id);

            return View(PostComment);
        }

        [Route("Edit/{id:int}")]
        [HttpPost]
        public JsonResult Edit(int id, PostComment comment)
        {
            try
            {
                if (comment.CommentId != id) throw new Exception("invalid comment");
                comment.AuthorId = 1;
                comment.DateTime = DateTime.Now;
                // TODO: Add update logic here

                _PostCommentOperations.Update(comment);
                return Json(true, JsonRequestBehavior.AllowGet);
                // TODO: Return to detail
                //  return RedirectToAction("Details", new { id });
            }
            catch(Exception ex)
            {
                // should throw exeption and use the logging
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("Delete/{id:int}")]
        [HttpPost]
        public ActionResult Delete(int id, int postId)
        {
            try
            {
               _PostCommentOperations.Delete(id);

                //CHECK: should return to blogs
                return RedirectToAction("Index", new { postId = postId });

            }
            catch
            {
                return this.HttpNotFound();
            }
        }


    }
}
