using Domain.Entities;
using Infrastructure.Persistence;

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
        var itemFromDb = _db.Items.Find(item.Id);
        if (itemFromDb == null)
            return;

        itemFromDb.Name = item.Name;
        itemFromDb.Description = item.Description;
        itemFromDb.Price = item.Price;
        itemFromDb.CategoryId = item.CategoryId;
        itemFromDb.ExpirationDate = item.ExpirationDate;

        if (item.ImageUrl != null)
            itemFromDb.ImageUrl = item.ImageUrl;
    }

    public void UpdateSoldStatus(int itemId, bool isSold)
    {
        var item = _db.Items.Find(itemId);
        if (item == null)
            return;

        item.Sold = isSold;
    }

    public async Task<bool> UserOwnsItemAsync(int itemId, string userId)
    {
        var item = await _db.Items.FindAsync(itemId);

        if (item == null)
            return false;

        if (item.SellerId != userId)
            return false;

        return true;
    }
}