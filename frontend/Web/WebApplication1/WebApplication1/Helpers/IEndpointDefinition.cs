namespace WebApplication1.EndPoints
{
    public interface IEndpointDefinition
    {
        void DefineEndpoints(WebApplication app);
        void DefineServices(IServiceCollection services);

    }
}
