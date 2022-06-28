using Domain.Entities;

namespace Infrastructure.Repository;

public interface IItemRepository : IRepository<Item>
{
    void Update(Item item);
    Task<bool> UserOwnsItemAsync(int itemId, string userId);
    void UpdateSoldStatus(int itemId, bool isSold);
    Task<bool> ItemExistsInOrder(int itemId);
}