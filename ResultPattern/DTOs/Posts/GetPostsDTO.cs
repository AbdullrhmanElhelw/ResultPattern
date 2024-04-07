namespace ResultPattern.DTOs.Posts;

public sealed record GetPostsDTO
    (Guid Id,
     string Title,
     string Content,
     DateTime CreatedAt);