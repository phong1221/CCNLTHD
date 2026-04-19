using Backend.Data;
using Backend.DTO;
using Backend.DTO.ReactDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ReactService : IReactionService
    {
        public readonly BlogDbContext context;
        public readonly IAuthService authService;

        public ReactService(BlogDbContext context, IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }

        public ReactionResponse create(ReactionRequest reactionRequest)
        {
            int UserId = authService.GetCurrentUser().Id;
            var post= context.Posts.FirstOrDefault(p => p.Id==reactionRequest.PostId && p.IsDeleted==false);
            if (post == null) {

                throw new Exception("post khong ton tai");
            }
            var reactold=context.Reactions.FirstOrDefault(r=>r.UserId == UserId && r.PostId==reactionRequest.PostId);
            if (reactold == null) {
                Reaction reactnew = new Reaction
                {
                    PostId = reactionRequest.PostId,
                    UserId = UserId,
                    ReactType = reactionRequest.ReactType,
                    CreatedAt = DateTime.Now,
                };
                context.Reactions.Add(reactnew);
                context.SaveChanges();
                reactnew.Post = post;
                return mapToResponse(reactnew);
            }
            reactold.ReactType = reactionRequest.ReactType;
            context.SaveChanges();
            // gán thêm data post vì add chỉ load được id của post thôi
            reactold.Post = post;
            return mapToResponse(reactold);
        }

        public void Delete(int PostId)
        {
            int UserId = authService.GetCurrentUser().Id;
            var react=context.Reactions.FirstOrDefault(r=>r.UserId==UserId&&r.PostId==PostId);
            if (react == null)
            {
                throw new Exception("người dùng chưa tương tác với bài viết này");
            }
            if (!authService.IsAuthor(react.UserId))
            {
                throw new Exception("bạn không có quyền xóa tương tác này");
            }
            context.Reactions.Remove(react);
            context.SaveChanges();
        }

        public List<ReactionResponse> GetAllByPost(int PostId)
        {
            var item= context.Reactions
                .Where(p => p.PostId == PostId)
                .Include(r => r.Post)
                .Include(r => r.User)
                .ToList();
            var result=item.Select(r => mapToResponse(r)).ToList();
            return result;
        }

        public PageResult<ReactionResponse> GetPageByPost(int PostId, int page, int pageSize)
        {
            var total = context.Reactions
                .Where(p=>p.PostId==PostId)
                .Count();
             var item= context.Reactions
                .Where(p => p.PostId == PostId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.Post)
                .Include(r => r.User)
                .ToList();
            var result= item.Select(r => mapToResponse(r)).ToList();
            return new PageResult<ReactionResponse>
            {
                Items = result,
                Total = total,
            };
        }
        public  ReactionResponse mapToResponse(Reaction reaction)
        {
            return new ReactionResponse
            {
                Id = reaction.Id,
                PostId = reaction.PostId,
                UserId = reaction.UserId,
                CreatedAt = reaction.CreatedAt,
                ReactType=reaction.ReactType,
                UserName=reaction.User?.UserName,
                PostContent=reaction.Post?.Content,
                PostTitle=reaction.Post?.Title,
            };
        }
    }
}
