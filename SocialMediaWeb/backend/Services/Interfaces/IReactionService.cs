using Backend.DTO;

using Backend.DTO.ReactDTO;

namespace Backend.Services.Interfaces
{
    public interface IReactionService
    {
        List<ReactionResponse> GetAllByPost(int PostId);
        PageResult<ReactionResponse> GetPageByPost(int PostId,int page, int pageSize);
        ReactionResponse create(ReactionRequest reactionRequest);
        void Delete(int  PostId);
    }
}
