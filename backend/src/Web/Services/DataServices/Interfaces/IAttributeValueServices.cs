using Domain.Entities;
using Web.Contracts.V1.Requests;


namespace Web.Services;
public interface IAttributeValueServices
{
    Task<List<AttributeValue>> Create(List<CreateAttributeValueRequest> requestAttributes);
    Task<bool> Delete(int attributeValueId);
    Task<bool> DeleteRange(int itemId);
    Task<List<AttributeValue>> GetItemAttributes(int itemId);

    Task<AttributeValue> Update(int attributeValueId, UpdateAttributeValueRequest request);

}