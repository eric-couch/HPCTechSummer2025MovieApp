using HPCTechSummer2025MovieApp.Data;

namespace HPCTechSummer2025MovieApp.Services;

public interface IUserService
{
    Task<ApplicationUser> GetUserByUserNameAsync(string userName);
}
