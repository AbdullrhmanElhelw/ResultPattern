using ResultPattern.Data;
using ResultPattern.Shared.UOW;

namespace ResultPattern.Shared;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> CommitAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();

    public Task RollbackAsync()
    {
        _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }
}