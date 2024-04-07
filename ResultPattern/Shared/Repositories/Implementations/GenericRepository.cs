using Microsoft.EntityFrameworkCore;
using ResultPattern.Data;
using ResultPattern.Shared.Repositories.Abstractions;

namespace ResultPattern.Shared.Repositories.Implementations;

public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(TModel entity) => _context.Set<TModel>().Add(entity);

    public void Delete(TModel entity) => _context.Set<TModel>().Remove(entity);

    public IEnumerable<TModel> FindAll(Func<TModel, bool> key) => _context
            .Set<TModel>()
            .AsNoTracking()
            .Where(key)
            .ToList();

    public IEnumerable<TModel> GetAll() =>
        _context.Set<TModel>()
        .AsNoTracking()
        .ToList();

    public TModel? GetById(Guid id) => _context.Set<TModel>().Find(id);

    public void Update(TModel entity) => _context.Set<TModel>().Update(entity);
}