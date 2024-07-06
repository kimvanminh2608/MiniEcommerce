using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> RegisterUser(User user);
    }
}
