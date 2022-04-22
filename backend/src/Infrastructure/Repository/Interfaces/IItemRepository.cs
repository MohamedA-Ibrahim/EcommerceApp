using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        void Update(Item item);
        Task<bool> UserOwnsItemAsync(int itemId, string? userId);
    }
}
