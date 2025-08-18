using HPCTechSummer2025MovieApp.Data;
using HPCTechSummer2025MovieApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPCTechSummer2025MovieApp.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<IActionResult> GetMovies()
    {
        var userName = User.Identity?.Name;

        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var userDto = await _context.Users
                         .Where(u => u.Id == user.Id)
                         .Select(u => new UserDTO
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             FavoriteMovies = u.FavoriteMovies.Select(um => um.Movie).ToList()
                         }).FirstOrDefaultAsync();

        if (userDto is null)
        {
            return NotFound("User data not found");
        }
        return Ok(userDto);
    }

}
