using Domain.Entities;

namespace Infrastructure.Repository;

public interface IAttributeValueRepository : IRepository<AttributeValue>
{
    Task AddRangeAsync(List<AttributeValue> attributes);
    void Update(AttributeValue attributeValue);
}