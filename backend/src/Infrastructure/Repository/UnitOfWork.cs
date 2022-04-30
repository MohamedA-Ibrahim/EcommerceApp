using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Item = new ItemRepository(_db);
    }

    public ICategoryRepository Category { get; }
    public IItemRepository Item { get; }

    public async Task SaveAsync()
    {
       await _db.SaveChangesAsync();
    }
}