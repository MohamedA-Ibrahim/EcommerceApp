namespace WebApplication1.EndPoints
{
    public class SwaggerEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s=> s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookstoreApi"));
        }

        public void DefineServices(IServiceCollection services)
        {
          services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "bookApi",
                    Version = "v1"
                });
            });
         }
    }
}
