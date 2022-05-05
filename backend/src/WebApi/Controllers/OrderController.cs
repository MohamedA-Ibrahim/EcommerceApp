using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Contracts.V1.Responses;
using Application.Contracts.V1.Responses.Wrappers;
using Application.Helpers;

namespace WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uriService = uriService;
        }


        [HttpGet(ApiRoutes.Orders.GetUserOrders)]
        public async Task<IActionResult> GetUserOrders([FromQuery] PaginationFilter paginationFilter)
        {
            var categories = await _unitOfWork.Category.GetAllAsync(null, paginationFilter);
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
            }

            var totalRecords = await _unitOfWork.Category.CountAsync();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(categoryResponse, paginationFilter, totalRecords, _uriService);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] int categoryId)
        {
            var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

            if (category == null)
                return NotFound();

            return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
        }

        [HttpPost(ApiRoutes.Orders.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest categoryRequest)
        {
            var category = new Category { Name = categoryRequest.Name };

            await _unitOfWork.Category.AddAsync(category);
            await _unitOfWork.SaveAsync();

            var locationUri = _uriService.GetCategoryUri(category.Id.ToString());
            return Created(locationUri, new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
        }

        [HttpPut(ApiRoutes.Orders.Update)]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

            if (category == null)
                return NotFound();

            category.Name = request.Name;

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
        }
    }
}
