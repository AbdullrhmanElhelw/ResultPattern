using ResultPattern.Entites.Base;
using ResultPattern.Utility;

namespace ResultPattern.Entites;

public sealed class Comment : Entity, IAuditableEntity, ISoftDeletableEntity
{
    private Comment(string content, Post post)
    {
        Ensure.NotNull(content, "Content is Null", nameof(content));
        Ensure.NotEmpty(content, "Should Not be Empty", nameof(content));
        Content = content;
        Post = post;
        CreatedOnUtc = DateTime.UtcNow;
        IsDeleted = false;
    }

    private Comment()
    { }

    public string Content { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public Guid PostId { get; private set; }
    public Post Post { get; private set; }

    public static Comment CreateComment(string content, Post post)
        => new(content, post);

    public static Comment UpdateComment(Post post, Comment comment, string content)
    {
        Ensure.NotNull(content, "Content is Null", nameof(content));
        Ensure.NotEmpty(content, "Should Not be Empty", nameof(content));
        comment.Post = post;
        comment.Content = content;
        comment.ModifiedOnUtc = DateTime.UtcNow;
        return comment;
    }

    public static Comment DeleteComment(Post post, Comment comment)
    {
        comment.Post = post;
        comment.IsDeleted = true;
        comment.DeletedOnUtc = DateTime.UtcNow;
        return comment;
    }
}