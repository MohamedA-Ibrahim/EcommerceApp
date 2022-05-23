using Domain.Entities;

namespace Infrastructure.Repository;

public interface IAttributeValueRepository : IRepository<AttributeValue>
{
    void Update(AttributeValue attributeValue);
}