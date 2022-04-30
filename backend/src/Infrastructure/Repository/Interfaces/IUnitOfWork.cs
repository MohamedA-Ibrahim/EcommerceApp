namespace Infrastructure.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IItemRepository Item { get; }
    Task SaveAsync();
}