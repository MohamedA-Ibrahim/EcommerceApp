using Domain.Entities;

namespace Infrastructure.Repository;

public interface IAttributeTypeRepository : IRepository<AttributeType>
{
    void Update(AttributeType attributeType);
}