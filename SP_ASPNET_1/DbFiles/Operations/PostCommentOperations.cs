using SP_ASPNET_1.DbFiles.UnitsOfWork;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using SP_ASPNET_1.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SP_ASPNET_1.DbFiles.Operations
{
    public interface IPostCommentOperations {

        Task<PagedResult<PostComment>> GetCommentsAsync(int postId, int page, int pageSize);
        IEnumerable<PostComment> GetCommentsPerPost(int postId);

        PostComment GetCommentById(int id);
        void Create(PostComment comment);
        void Update(PostComment comment);
        void Delete(int id);

    }
    public class PostCommentOperations : IPostCommentOperations
    {

        private IUnitOfWork _unitOfWork;//= new UnitOfWork();
        public PostCommentOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
     

      

        public void Create(PostComment comment)
        {
            try
            {
                this._unitOfWork.PostCommentRepository.Insert(comment);
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
                PostComment comment = this.GetCommentById(id);
                this._unitOfWork.PostCommentRepository.Remove(comment);
                this._unitOfWork.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public Task<PagedResult<PostComment>> GetCommentsAsync(int postId ,int page, int pageSize)
        {
            // 1 will be changed
            return _unitOfWork.PostCommentRepository.GetAsync(b=>b.PostId==postId, b => b.OrderByDescending(d => d.DateTime), "",page,pageSize);
        }
  

        public PostComment GetCommentById(int id)
        {
            return _unitOfWork.PostCommentRepository.GetByID(id);
        }

        IEnumerable<PostComment> IPostCommentOperations.GetCommentsPerPost(int postId)
        {
            return _unitOfWork.PostCommentRepository.Get(b => b.PostId == postId, b => b.OrderByDescending(d => d.DateTime), "");

        }

        public void Update(PostComment comment)
        {
                _unitOfWork.PostCommentRepository.Update(comment);
            this._unitOfWork.Save();
        }
    }
}