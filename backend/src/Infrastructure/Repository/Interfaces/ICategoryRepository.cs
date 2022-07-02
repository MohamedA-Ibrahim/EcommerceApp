using Domain.Entities;

namespace Infrastructure.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
    Task<bool> CategoryHasItems(int categoryId);
}