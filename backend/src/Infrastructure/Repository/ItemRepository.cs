using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ItemRepository : Repository<Item>, IItemRepository
{
    private readonly ApplicationDbContext _db;

    public ItemRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Item item)
    {
        var itemFromDb = _db.Items.FirstOrDefault(i => i.Id == item.Id);
        if (itemFromDb == null)
            return;

        itemFromDb.Name = item.Name;
        itemFromDb.Description = item.Description;
        itemFromDb.Price = item.Price;
        itemFromDb.CategoryId = item.CategoryId;
        itemFromDb.ExpirationDate = item.ExpirationDate;

        if (item.Image != null) itemFromDb.Image = item.Image;
    }

    public async Task<bool> UserOwnsItemAsync(int itemId, string? userId)
    {
        var item = await _db.Items.AsNoTracking().SingleOrDefaultAsync(x => x.Id == itemId);

        if (item == null) return false;

        if (item.CreatedBy != userId) return false;

        return true;
    }
}