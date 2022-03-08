using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Repo.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        #region property  
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;
        #endregion

        #region Constructor  
        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            entities = _context.Set<T>();
        }
        #endregion

        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public T GetById(int id)
        {
            return entities.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await entities.FindAsync(id);
        }

        public void Add(T entity)
        {
            entities.Add(entity);
            _context.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await entities.AddAsync(entity);
            _context.SaveChanges();          
        }

        public void Update(T entity)
        {
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
