namespace ApiGateway.Dtos.AuthDtos
{
    public sealed record RegisterDto
    (
        string UserName,
        string Password
    );
}
