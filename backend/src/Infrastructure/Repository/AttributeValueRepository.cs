using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class AttributeTypeRepository : Repository<AttributeType>, IAttributeTypeRepository
{
    private readonly ApplicationDbContext _db;

    public AttributeTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(AttributeType attributeType)
    {
        _db.AttributeTypes.Update(attributeType);
    }
}