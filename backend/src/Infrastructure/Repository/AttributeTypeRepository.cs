using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class AttributeValueRepository : Repository<AttributeValue>, IAttributeValueRepository
{
    private readonly ApplicationDbContext _db;

    public AttributeValueRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(AttributeValue attributeValue)
    {
        _db.AttributeValues.Update(attributeValue);
    }

    public async Task AddRangeAsync(List<AttributeValue> attributes)
    {
        await _db.AttributeValues.AddRangeAsync(attributes);
    }
}