using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private ApplicationDbContext _db;

        public ItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Item item)
        {
            var itemFromDb = _db.Items.FirstOrDefault(i => i.Id == item.Id);
            if (itemFromDb == null)
                return;

            itemFromDb.Name = item.Name;
            itemFromDb.Description = item.Description;
            itemFromDb.Price = item.Price;
            itemFromDb.CategoryId = item.CategoryId;
            itemFromDb.ExpirationDate = item.ExpirationDate;

            if (item.Image != null)
            {
                itemFromDb.Image = item.Image;
            }

            //We can use this directly if we want to update everything
            //but here we don't update image so we don't call this
            //_db.Items.Update(item);
        }
    }
}
