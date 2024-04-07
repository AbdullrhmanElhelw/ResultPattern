namespace ResultPattern.DTOs.Comments;

public sealed record ReadCommentDTO
    (Guid PostId, Guid CommentId, string Content);