using Domain.Entities;

namespace Infrastructure.Repository;

public interface IItemRepository : IRepository<Item>
{
    void Update(Item item);
    Task<bool> UserOwnsItemAsync(int itemId, string? userId);
}