using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly WebAppContext _context;
        public UserService(WebAppContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUser (User user)
        {
            try
            {
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    return false;
                }

                _context.Users.Add(user);
                var result = await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while saving the user.", ex);
            }
            
        }
    }
}
