using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class CartRepository : Repository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _db;

    public CartRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}