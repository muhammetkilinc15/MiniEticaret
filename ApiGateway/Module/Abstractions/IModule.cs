namespace ApiGateway.Module.Abstractions
{
    public interface IModule
    {
        void AddRoutes(IEndpointRouteBuilder app);
    }
}
