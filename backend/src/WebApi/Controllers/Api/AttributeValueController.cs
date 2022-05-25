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
using Web.Services;

namespace Web.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class AttributeValueController : ControllerBase
{
    private readonly IAttributeValueServices _services;
    private readonly IMapper _mapper;

    public AttributeValueController(IAttributeValueServices services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    /// <summary>
    /// Add attribute values for an item (removes old attributes for an item before adding)
    /// </summary>
    /// <param name="requestAttributes"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.AttributeValues.Create)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Create([FromRoute]int itemId, [FromBody] List<CreateAttributeValueRequest> requestAttributes)
    {
        List<AttributeValue> attributeValues = await _services.Create(itemId, requestAttributes);
        return Ok(_mapper.Map<List<AttributeValueResponse>>(attributeValues));
    }

    [HttpGet(ApiRoutes.AttributeValues.GetByItemId)]
    public async Task<IActionResult> GetItemAttributes([FromRoute] int itemId)
    {
        var itemAttributes = await _services.GetItemAttributes(itemId);
        var itemAttributesResponse = _mapper.Map<List<AttributeValueResponse>>(itemAttributes);
        return Ok(itemAttributesResponse);
    }

    [HttpPut(ApiRoutes.AttributeValues.Update)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update([FromRoute] int attributeValueId, UpdateAttributeValueRequest request)
    {
        var attributeValue = await _services.Update(attributeValueId, request);

        if (attributeValue == null)
            return NotFound();
        
        return Ok(_mapper.Map<AttributeValueResponse>(attributeValue));
    }

}