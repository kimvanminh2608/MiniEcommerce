using WebApp.Models;

namespace WebApp.JwtService
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
