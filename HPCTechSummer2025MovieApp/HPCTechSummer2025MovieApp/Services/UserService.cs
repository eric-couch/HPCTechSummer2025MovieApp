using HPCTechSummer2025MovieApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HPCTechSummer2025MovieApp.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }
}
