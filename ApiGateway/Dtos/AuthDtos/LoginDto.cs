namespace ApiGateway.Dtos.AuthDtos
{
    public sealed record LoginDto
       (
           string userName,
           string Password
       );
}
