using Backend.Data;
using Backend.DTO;
using Backend.DTO.CommentDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Backend.Services
{
    public class CommentService : ICommentService
    {
        private readonly BlogDbContext context;
        public readonly IAuthService authService;

        public CommentService(BlogDbContext context, IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }

        public List<CommentResponse> GetAll()
        {
            return context.Comments
                .Where(c => c.IsDeleted == false)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    Content = c.Content,
                    IsDeleted = c.IsDeleted,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                }).ToList();
        }

        public CommentResponse GetById(int id)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (comment == null)
            {
                throw new Exception("Bình luận không tồn tại với id: " + id);
            }
            return MapToResponse(comment);
        }

        public CommentResponse Create(CommentRequest request)
        {
            var userId=authService.GetCurrentUser().Id;
            // Kiểm tra Post tồn tại
            var post = context.Posts.FirstOrDefault(p => p.Id == request.PostId && p.IsDeleted == false);
            if (post == null)
            {
                throw new Exception("Bài viết không tồn tại với id: " + request.PostId);
            }

            Comment comment = new Comment
            {
                UserId = userId,
                PostId = request.PostId,
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            context.Comments.Add(comment);
            context.SaveChanges();
            return MapToResponse(comment);
        }

        public CommentResponse Update(int id, CommentRequest request)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (comment == null)
            {
                throw new Exception("Bình luận không tồn tại với id: " + id);
            }
            if (!authService.IsAuthor(comment.UserId))
            {
                throw new Exception("bạn không có quyền sửa bình luận này");
            }
            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.Now;
            context.SaveChanges();
            return MapToResponse(comment);
        }

        public void Delete(int id)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (comment == null)
            {
                throw new Exception("Bình luận không tồn tại với id: " + id);
            }
            if (!authService.IsAuthor(comment.UserId))
            {
                throw new Exception("bạn không có quyền xóa bình luận này");
            }
            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public List<CommentResponse> Search(string keyword)
        {
            var lowerKeyword = keyword.ToLower();
            var item= context.Comments
                .Where(c => c.IsDeleted == false &&
                    c.Content.ToLower().Contains(lowerKeyword))
                .ToList();
             var result=item.Select(c=>MapToResponse(c)).ToList();
            return result;
        }

        public PageResult<CommentResponse> GetPage(int page, int pageSize)
        {
            var items = context.Comments
                .Where(c => c.IsDeleted == false)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = items.Select(c => MapToResponse(c)).ToList();

            return new PageResult<CommentResponse>
            {
                Items = result,
                Total = context.Comments.Count(c => c.IsDeleted == false)
            };
        }

        private static CommentResponse MapToResponse(Comment comment)
        {
            return new CommentResponse
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Content = comment.Content,
                IsDeleted = comment.IsDeleted,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
            };
        }
    }
}
