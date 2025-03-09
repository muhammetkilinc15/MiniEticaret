using ApiGateway.Models;

namespace ApiGateway.Services
{
    public interface IJwtProvider
    {
        string CreateToken(User user);
    }
}
