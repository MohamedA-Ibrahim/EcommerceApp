using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Services.DataServices.Interfaces;

namespace Ecommerce.UnitTests
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> service = new();
        private readonly Random random = new();

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = 1,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = random.Next(1000),
                ExpirationDate = DateTime.UtcNow,
                CategoryId = 1,
                SellerId = Guid.NewGuid().ToString(),
            };
        }
    }
}
