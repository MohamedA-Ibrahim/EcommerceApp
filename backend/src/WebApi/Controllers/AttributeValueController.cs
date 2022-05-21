using System.Net.Mime;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using AutoMapper;
using Application.Interfaces;
using Application.Contracts.V1.Responses;


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

    /// <summary>
    /// Add attribute values for an item (removes old attributes for an item before adding)
    /// </summary>
    /// <param name="requestAttributes"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.AttributeValues.Create)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Create([FromBody] List<CreateAttributeValueRequest> requestAttributes)
    {
        //TODO: change the itemId in the request to FromRoute
        //Delete any old existing attribute values for an item
        var oldAttributes = await _unitOfWork.AttributeValue.GetAllAsync(x=> x.ItemId == requestAttributes.FirstOrDefault().ItemId);
        if(oldAttributes.Count != 0)
        {
            _unitOfWork.AttributeValue.RemoveRange(oldAttributes);
            await _unitOfWork.SaveAsync();
        }

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

    [HttpPut(ApiRoutes.AttributeValues.Update)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Update([FromRoute] int attributeValueId, UpdateAttributeValueRequest request)
    {
        var attributeValue = await _unitOfWork.AttributeValue.GetFirstOrDefaultAsync(attributeValueId);

        if (attributeValue == null)
            return NotFound();

        attributeValue.Value = request.Value;

        _unitOfWork.AttributeValue.Update(attributeValue);
        await _unitOfWork.SaveAsync();

        return Ok(attributeValue);
    }

}