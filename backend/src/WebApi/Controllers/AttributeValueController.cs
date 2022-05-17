﻿using System.Net.Mime;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Models;
using AutoMapper;
using Application.Interfaces;
using Application.Contracts.V1.Responses;
using Application.Contracts.V1.Responses.Wrappers;
using Application.Helpers;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class AttributeValueController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public AttributeValueController(IUnitOfWork unitOfWork,  IUriService uriService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _uriService = uriService;
        _mapper = mapper;
    }

    [HttpPost(ApiRoutes.AttributeValues.Create)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Create([FromBody] List<CreateAttributeValueRequest> requestAttributes)
    {
        List<AttributeValue> attributeValues = new();

        foreach (var attribute in requestAttributes)
        {
            var attributeValue = new AttributeValue
            {
                ItemId = attribute.ItemId,
                AttributeTypeId = attribute.AttributeTypeId,
                Value = attribute.AttributeValue
            };

            attributeValues.Add(attributeValue);
        }

        await _unitOfWork.AttributeValue.AddRangeAsync(attributeValues);
        await _unitOfWork.SaveAsync();

        return Ok(attributeValues);
    }

    [HttpGet(ApiRoutes.AttributeValues.GetByItemId)]
    public async Task<IActionResult> GetItemAttributes([FromRoute] int itemId)
    {
        var itemAttributes = await _unitOfWork.AttributeValue.GetAllIncludingAsync(x=> x.ItemId == itemId, null, x=> x.AttributeType);
        var itemAttributesResponse = _mapper.Map<List<AttributeValueResponse>>(itemAttributes);

        return Ok(itemAttributesResponse);
    }

}