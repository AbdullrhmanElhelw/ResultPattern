namespace ResultPattern.Shared.UOW;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();

    Task RollbackAsync();
}