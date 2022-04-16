using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.EndPoints
{
    public class CategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/categories", GetAll);
            app.MapPost("/categories", Create);
            app.MapPut("/categories/{id}", Update);
            app.MapDelete("/categories/{id}", Delete);
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddScoped<CategoryReposiory>();
        }

        internal List<Category> GetAll(CategoryReposiory repo)
        {
             return repo.GetAll();
        }

        internal void Create(Category cat, CategoryReposiory repo)
        {
            repo.Create(cat);
        }

        internal void Update (int id, Category cat, CategoryReposiory repo)
        {
            repo.Update(id, cat);
        }

        internal void Delete(int id, CategoryReposiory repo)
        {
            repo.Delete(id);
        }


    }
}
