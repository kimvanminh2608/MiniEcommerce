using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(User user);
    }
}
