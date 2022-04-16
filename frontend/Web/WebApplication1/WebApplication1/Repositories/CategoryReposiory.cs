using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CategoryReposiory
    {
        private ApplicationDbContext _context;

        public CategoryReposiory(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public void Create(Category cat)
        {
            _context.Add(cat);
            _context.SaveChanges();
        }

        public void Update(int id, Category cat)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return ;

            category.Name = cat.Name;

            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return;
         
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
