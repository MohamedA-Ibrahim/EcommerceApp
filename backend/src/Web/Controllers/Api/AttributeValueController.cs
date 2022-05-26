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
    private readonly IAttributeValueServices _attributeValueService;
    private readonly IMapper _mapper;

    public AttributeValueController(IAttributeValueServices attributeValueService, IMapper mapper)
    {
        _attributeValueService = attributeValueService;
        _mapper = mapper;
    }

    /// <summary>
    /// Add attribute values for an item
    /// </summary>
    /// <param name="requestAttributes"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.AttributeValues.Create)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Create([FromBody] List<CreateAttributeValueRequest> requestAttributes)
    {
        List<AttributeValue> attributeValues = await _attributeValueService.Create(requestAttributes);
        return Ok(_mapper.Map<List<AttributeValueResponse>>(attributeValues));
    }

    [HttpGet(ApiRoutes.AttributeValues.GetByItemId)]
    public async Task<IActionResult> GetItemAttributes([FromRoute] int itemId)
    {
        var itemAttributes = await _attributeValueService.GetItemAttributes(itemId);
        var itemAttributesResponse = _mapper.Map<List<AttributeValueResponse>>(itemAttributes);
        return Ok(itemAttributesResponse);
    }

    [HttpPut(ApiRoutes.AttributeValues.Update)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update([FromRoute] int attributeValueId, UpdateAttributeValueRequest request)
    {
        var attributeValue = await _attributeValueService.Update(attributeValueId, request);

        if (attributeValue == null)
            return NotFound();
        
        return Ok(_mapper.Map<AttributeValueResponse>(attributeValue));
    }

    /// <summary>
    /// Delete a single attribute value by its id
    /// </summary>
    /// <param name="attributeValueId">The id of the attribute value to delete</param>
    ///<response code="204">Deleted Successfully</response>

    [HttpDelete(ApiRoutes.AttributeValues.Delete)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Delete([FromRoute] int attributeValueId)
    {
        var deleted = await _attributeValueService.Delete(attributeValueId);

        if (!deleted)
            return BadRequest();

        return NoContent();
    }

    /// <summary>
    /// Delete attribute values by itemId
    /// </summary>
    /// <param name="itemId">The Id of the item to deltee an attribute value for</param>
    ///<response code="204">Deleted Successfully</response>
    [HttpDelete(ApiRoutes.AttributeValues.DeleteRange)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteRange([FromRoute] int itemId)
    {
        var deleted = await _attributeValueService.DeleteRange(itemId);

        if (!deleted)
            return BadRequest();

        return NoContent();
    }

}