using ResultPattern.Data;
using ResultPattern.Entites;
using ResultPattern.Shared.Repositories.Abstractions;

namespace ResultPattern.Shared.Repositories.Implementations;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }
}