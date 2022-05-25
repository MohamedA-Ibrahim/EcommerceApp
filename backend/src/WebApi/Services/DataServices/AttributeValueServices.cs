using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;


namespace Web.Services;

public class AttributeValueServices : IAttributeValueServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUriService _uriService;

    public AttributeValueServices(IUnitOfWork unitOfWork, IUriService uriService)
    {
        _unitOfWork = unitOfWork;
        _uriService = uriService;
    }

    /// <summary>
    /// Add attribute values for an item (removes old attributes for an item before adding)
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="requestAttributes"></param>
    /// <returns></returns>
    public async Task<List<AttributeValue>> Create(int itemId, List<CreateAttributeValueRequest> requestAttributes)
    {
        //Delete any old existing attribute values for an item
        var oldAttributes = await _unitOfWork.AttributeValue.GetAllAsync(x => x.ItemId == itemId);
        if (oldAttributes.Count != 0)
        {
            _unitOfWork.AttributeValue.RemoveRange(oldAttributes);
            await _unitOfWork.SaveAsync();
        }

        List<AttributeValue> attributeValues = new();

        foreach (var attribute in requestAttributes)
        {
            var attributeValue = new AttributeValue
            {
                ItemId = itemId,
                AttributeTypeId = attribute.AttributeTypeId,
                Value = attribute.AttributeValue
            };

            attributeValues.Add(attributeValue);
        }

        await _unitOfWork.AttributeValue.AddRangeAsync(attributeValues);
        await _unitOfWork.SaveAsync();

        return attributeValues;
    }
    public async Task<List<AttributeValue>> GetItemAttributes(int itemId)
    {
        var itemAttributes = await _unitOfWork.AttributeValue.GetAllIncludingAsync(x => x.ItemId == itemId, null, x => x.AttributeType);
        return itemAttributes;
    }

    public async Task<AttributeValue> Update(int attributeValueId, UpdateAttributeValueRequest request)
    {
        var attributeValue = await _unitOfWork.AttributeValue.GetFirstOrDefaultAsync(attributeValueId);

        if (attributeValue == null)
            return null;

        attributeValue.Value = request.Value;

        _unitOfWork.AttributeValue.Update(attributeValue);
        await _unitOfWork.SaveAsync();


        return attributeValue;
    }

}