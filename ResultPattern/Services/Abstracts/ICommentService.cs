using ResultPattern.DTOs.Comments;
using ResultPattern.Shared;

namespace ResultPattern.Services.Abstracts;

public interface ICommentService
{
    Task<Result> CreateComment(CreateCommentDTO commentDto, CancellationToken cancellationToken);

    Task<Result> UpdateComment(UpdateCommentDTO commentDto, CancellationToken cancellationToken);

    Task<Result> DeleteComment(Guid postId, Guid commentId, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<ReadCommentDTO>>> GetCommentsAsync(Guid PostId);
}