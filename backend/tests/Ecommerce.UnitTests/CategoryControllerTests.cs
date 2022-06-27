using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Web.Contracts.V1.Responses;
using Web.Controllers;
using Web.Mapping;
using Web.Services.DataServices.Interfaces;
using Xunit;
using Application.Models;
using System.Collections.Generic;
using Web.Contracts.V1.Responses.Wrappers;

namespace Ecommerce.UnitTests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> serviceMock = new();
        private readonly Random random = new();

        public CategoryControllerTests()
        {
        }

        [Fact]
        public async Task GetCategoryAsync_WithUnexistingCategory_ReturnsNotFound()
        {
            // Arrange
            serviceMock.Setup(serv=> serv.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Category)null);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToResponseProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var controller = new CategoryController(mapper, serviceMock.Object);

            // Act
            var result = await controller.Get(0);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCategoryAsync_WithExistingCategory_ReturnsExpectedCategory()
        {
            // Arrange
            var expectedCategory = CreateRandomCategory();
            serviceMock.Setup(serv => serv.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedCategory);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToResponseProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var controller = new CategoryController(mapper, serviceMock.Object);

            // Act
            IActionResult result = await controller.Get(1);
            OkObjectResult okResult = result as OkObjectResult;
            CategoryResponse resultCategory = okResult.Value as CategoryResponse;

            // Assert
            resultCategory.Should().BeEquivalentTo(
                expectedCategory,
                options => options
                .Excluding(x => x.AttributeTypes)
                .Excluding(x => x.Items)
                .ComparingByMembers<CategoryResponse>());
        }

        #region Helper methods

        private Category CreateRandomCategory()
        {
            return new()
            {
                Id = random.Next(1000),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
        }

        #endregion
    }
}