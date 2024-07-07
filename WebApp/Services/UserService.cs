using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextOptions<WebAppContext> _context;
        public UserService(DbContextOptions<WebAppContext> context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUser (User user)
        {
            try
            {
                using (var context = new WebAppContext(_context))
                {
                    //if (context.Users.Any(u => u.Username == user.Username))
                    //{
                    //    return false;
                    //}

                    //context.Users.Add(user);
                    //var result = await context.SaveChangesAsync();

                    return true;
                }
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while saving the user.", ex);
            }
            
        }
    }
}
