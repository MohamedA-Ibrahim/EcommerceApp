using System.Net.Mime;
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
public class AttributeTypeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public AttributeTypeController(IUnitOfWork unitOfWork,  IUriService uriService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _uriService = uriService;
        _mapper = mapper;
    }

    /// <summary>
    ///  Get All Attribute types for category
    /// </summary>
    /// <returns>List of attribute types</returns>
    [HttpGet(ApiRoutes.AttributeTypes.GetByCategoryId)]
    public async Task<IActionResult> GetByCategoryAsync([FromRoute] int categoryId)
    {
        var attributeTypes = await _unitOfWork.AttributeType.GetAllAsync(x=> x.CategoryId == categoryId);

        var attributeTypsResponse = _mapper.Map<List<AttributeTypeResponse>>(attributeTypes);

        return Ok(attributeTypsResponse);
    }


    /// <summary>
    /// Create a new attribute type under a category
    /// </summary>
    /// <param name="request">The attribute type to create</param>
    ///<response code="200">Create Success</response>
    [HttpPost(ApiRoutes.AttributeTypes.Create)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAttributeTypeRequest request)
    {
        var attributeType = new AttributeType
        {
            CategoryId = request.CategoryId,
            Name = request.Name
        };

        await _unitOfWork.AttributeType.AddAsync(attributeType);
        await _unitOfWork.SaveAsync();

        var locationUri = _uriService.GetItemUri(attributeType.Id.ToString());
        return Created(locationUri, new Response<AttributeType>(_mapper.Map<AttributeType>(attributeType)));
    }

    /// <summary>
    /// Update an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to update</param>
    /// <param name="request">The updated info</param>
    ///<response code="200">Update Success</response>
    [HttpPut(ApiRoutes.AttributeTypes.Update)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int attributeTypeId, [FromBody] UpdateAttributeTypeRequest request)
    {
        var attributeType = await _unitOfWork.AttributeType.GetFirstOrDefaultAsync(attributeTypeId);

        if (attributeType == null) 
            return NotFound();

        attributeType.Name = request.Name;
      
        _unitOfWork.AttributeType.Update(attributeType);
        await _unitOfWork.SaveAsync();

        return Ok(new Response<AttributeType>(_mapper.Map<AttributeType>(attributeType)));
    }

    /// <summary>
    /// Delete an attribute type
    /// </summary>
    /// <param name="attributeTypeId">The id of the attribute type to delete</param>
    ///<response code="204">Delete Success</response>
    [HttpDelete(ApiRoutes.AttributeTypes.Delete)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int attributeTypeId)
    {
        var attributeType = await _unitOfWork.AttributeType.GetFirstOrDefaultAsync(attributeTypeId);

        if (attributeType == null) 
            return NotFound();

        _unitOfWork.AttributeType.Remove(attributeType);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }
}