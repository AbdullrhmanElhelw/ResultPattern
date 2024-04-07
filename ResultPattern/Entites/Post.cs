using ResultPattern.Entites.Base;
using ResultPattern.Utility;

namespace ResultPattern.Entites;

public sealed class Post : Entity, IAuditableEntity, ISoftDeletableEntity
{
    private List<Comment> _comments;

    private Post(string title, string content)
    {
        Ensure.NotEmpty(title, "Should Not be Empty", nameof(title));
        Ensure.NotNull(title, "Should Not be Null", nameof(title));
        Title = title;
        Content = content;
        _comments = [];
        CreatedOnUtc = DateTime.UtcNow;
        IsDeleted = false;
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsDeleted { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    public static Post Create(string title, string content)
        => new(title, content);

    public static Post Update(Post post, string title, string content)
    {
        Ensure.NotNull(post, "Post Should Not be Null", nameof(post));
        post.Title = title;
        post.Content = content;
        post.ModifiedOnUtc = DateTime.UtcNow;
        return post;
    }

    public static void Delete(Post post)
    {
        Ensure.NotNull(post, "Post Should Not be Null", nameof(post));
        post.IsDeleted = true;
        post.DeletedOnUtc = DateTime.UtcNow;
    }
}