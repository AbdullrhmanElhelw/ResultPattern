using ResultPattern.Data;
using ResultPattern.Entites;
using ResultPattern.Shared.Repositories.Abstractions;

namespace ResultPattern.Shared.Repositories.Implementations;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }
}