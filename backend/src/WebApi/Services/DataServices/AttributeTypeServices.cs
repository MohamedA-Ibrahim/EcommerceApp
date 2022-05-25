﻿using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;


namespace Web.Services;

public class AttributeTypeServices : IAttributeTypeServices
{
    private readonly IUnitOfWork _unitOfWork;

    public AttributeTypeServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///  Get All Attribute types for category
    /// </summary>
    /// <returns>List of attribute types</returns>
    public async Task<List<AttributeType>> GetByCategoryAsync(int categoryId)
    {
        return await _unitOfWork.AttributeType.GetAllAsync(x => x.CategoryId == categoryId); 
    }


    /// <summary>
    /// Create a new attribute type under a category
    /// </summary>
    /// <param name="request">The attribute type to create</param>
    public async Task<bool> Create(List<CreateAttributeTypeRequest> request)
    {
        List<AttributeType> attributeTypes = new();

        foreach (var attribute in request)
        {
            var attributeType = new AttributeType
            {
                CategoryId = attribute.CategoryId,
                Name = attribute.Name
            };
            attributeTypes.Add(attributeType);
        }

        await _unitOfWork.AttributeType.AddRangeAsync(attributeTypes);

        await _unitOfWork.SaveAsync();

        return true;
    }

    /// <summary>
    /// Update an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to update</param>
    /// <param name="request">The updated info</param>
    public async Task<AttributeType> Update( int attributeTypeId,UpdateAttributeTypeRequest request)
    {
        var attributeType = await _unitOfWork.AttributeType.GetFirstOrDefaultAsync(attributeTypeId);

        if (attributeType == null)
            return null;

        attributeType.Name = request.Name;

        _unitOfWork.AttributeType.Update(attributeType);
        await _unitOfWork.SaveAsync();

        return attributeType;
    }

    /// <summary>
    /// Delete an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to delete</param>
    public async Task<bool> Delete(int attributeTypeId)
    {
        var attributeType = await _unitOfWork.AttributeType.GetFirstOrDefaultAsync(attributeTypeId);

        if (attributeType == null)
            return false;

        _unitOfWork.AttributeType.Remove(attributeType);
        await _unitOfWork.SaveAsync();

        return true;
    }
}