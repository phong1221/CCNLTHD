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

        public ReactService(BlogDbContext context)
        {
            this.context = context;
        }

        public ReactionResponse create(ReactionRequest reactionRequest)
        {
            int UserId = 1;
            var reactold=context.Reactions.FirstOrDefault(r=>r.UserId == UserId && r.PostId==reactionRequest.PostId);
            if (reactold == null) {
                Reaction reactnew = new Reaction
                {
                    PostId = reactionRequest.PostId,
                    UserId = UserId,
                    CreatedAt = DateTime.Now,
                };
                context.Reactions.Add(reactnew);
                context.SaveChanges();
                return mapToResponse(reactnew);
            }
            reactold.ReactType = reactionRequest.ReactType;
            context.SaveChanges();
            return mapToResponse(reactold);
        }

        public void Delete(int PostId)
        {
            int UserId = 1;
            var react=context.Reactions.FirstOrDefault(r=>r.UserId==UserId&&r.PostId==PostId);
            if (react == null)
            {
                throw new Exception("người dùng chưa tương tác với bài viết này");
            }
            context.Reactions.Remove(react);
            context.SaveChanges();
        }

        public List<ReactionResponse> GetAllByPost(int PostId)
        {
            var item= context.Reactions
                .Where(p => p.Id == PostId)
                .Include(r => r.Post)
                .Include(r => r.User)
                .ToList();
            var result=item.Select(r => mapToResponse(r)).ToList();
            return result;
        }

        public PageResult<ReactionResponse> GetPageByPost(int PostId, int page, int pageSize)
        {
            var total = context.Reactions
                .Where(p=>p.Id==PostId)
                .Count();
             var item= context.Reactions
                .Where(p => p.Id == PostId)
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
