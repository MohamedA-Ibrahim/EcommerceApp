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
public interface IAttributeValueServices
{
    Task<List<AttributeValue>> Create(int itemId,List<CreateAttributeValueRequest> requestAttributes);

    Task<List<AttributeValue>> GetItemAttributes(int itemId);

    Task<AttributeValue> Update(int attributeValueId, UpdateAttributeValueRequest request);

}