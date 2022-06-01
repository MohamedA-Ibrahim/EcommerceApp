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
public class AttributeTypeController : ControllerBase
{
    private readonly IAttributeTypeServices _attributeTypeService;
    private readonly IMapper _mapper;

    public AttributeTypeController(IAttributeTypeServices attributeTypeService, IMapper mapper)
    {
        _attributeTypeService = attributeTypeService;
        _mapper = mapper;
    }

    /// <summary>
    ///  Get All Attribute types for category
    /// </summary>
    /// <returns>List of attribute types</returns>
    [HttpGet(ApiRoutes.AttributeTypes.GetByCategoryId)]
    public async Task<IActionResult> GetByCategoryAsync([FromRoute] int categoryId)
    {
        var attributeTypes = await _attributeTypeService.GetByCategoryAsync(categoryId);
        var attributeTypsResponse = _mapper.Map<List<AttributeTypeResponse>>(attributeTypes);
        return Ok(attributeTypsResponse);
    }


    /// <summary>
    /// Create a new attribute type under a category
    /// </summary>
    /// <param name="request">The attribute type to create</param>
    [HttpPost(ApiRoutes.AttributeTypes.Create)]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Create([FromBody] List<CreateAttributeTypeRequest> request)
    {
        var attributeTypes = await _attributeTypeService.Create(request);

        if (attributeTypes == null)
            return NotFound();

        return Ok(_mapper.Map<List<AttributeTypeResponse>>(attributeTypes));

    }

    /// <summary>
    /// Update an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to update</param>
    /// <param name="request">The updated info</param>
    ///<response code="200">Update Success</response>
    [HttpPut(ApiRoutes.AttributeTypes.Update)]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update([FromRoute] int attributeTypeId, [FromBody] UpdateAttributeTypeRequest request)
    {
        var attributeType = await _attributeTypeService.Update(attributeTypeId, request);

        if (attributeType == null)
            return NotFound();

        return Ok(_mapper.Map<AttributeTypeResponse>(attributeType));
    }

    /// <summary>
    /// Delete an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to delete</param>
    ///<response code="204">Deleted Successfully</response>
    [HttpDelete(ApiRoutes.AttributeTypes.Delete)]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Delete([FromRoute] int attributeTypeId)
    {
        var attributeType = await _attributeTypeService.Delete(attributeTypeId);
        if (!attributeType)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Delete attribute types by categoryId
    /// </summary>
    /// <param name="categoryId">The Id of the category to delete all attribute types</param>
    ///<response code="204">Deleted Successfully</response>
    [HttpDelete(ApiRoutes.AttributeTypes.DeleteRange)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteRange([FromRoute] int categoryId)
    {
        var deleted = await _attributeTypeService.DeleteRange(categoryId);

        if (!deleted)
            return BadRequest();

        return NoContent();
    }
}