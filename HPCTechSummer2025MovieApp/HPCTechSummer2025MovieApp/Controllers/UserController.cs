using HPCTechSummer2025MovieApp.Data;
using HPCTechSummer2025MovieApp.Model;
using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPCTechSummer2025MovieApp.Controllers;

[ApiController]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserController> _logger;

    public UserController(  UserManager<ApplicationUser> userManager, 
                            ApplicationDbContext context,
                            ILogger<UserController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<IActionResult> GetMovies()
    {
        var userName = User.Identity?.Name;

        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            _logger.LogWarning("No user found for {0}", userName);
            return NotFound("User not found");
        }

        var userDto = await _context.Users
                         .Where(u => u.Id == user.Id)
                         .Select(u => new UserDTO
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             FavoriteMovies = u.FavoriteMovies
                                .Select(fm => new MovieDto
                                {
                                    imdbID = fm.MovieId,
                                    Title = fm.Movie.Title,
                                    Year = fm.Movie.Year,
                                    Rated = fm.Movie.Rated,
                                    Released = fm.Movie.Released,
                                    Runtime = fm.Movie.Runtime,
                                    Genre = fm.Movie.Genre,
                                    Director = fm.Movie.Director,
                                    Writer = fm.Movie.Writer,
                                    Actors = fm.Movie.Actors,
                                    Plot = fm.Movie.Plot,
                                    Language = fm.Movie.Language,
                                    Country = fm.Movie.Country,
                                    Awards = fm.Movie.Awards,
                                    Poster = fm.Movie.Poster,
                                    Metascore = fm.Movie.Metascore,
                                    imdbRating = fm.Movie.imdbRating,
                                    imdbVotes = fm.Movie.imdbVotes,
                                    Type = fm.Movie.Type,
                                    DVD = fm.Movie.DVD,
                                    BoxOffice = fm.Movie.BoxOffice,
                                    Production = fm.Movie.Production,
                                    Website = fm.Movie.Website,
                                    Response = fm.Movie.Response
                                }).ToList()
                         }).FirstOrDefaultAsync();

        if (userDto is null)
        {
            _logger.LogError("No user data found for user id: {0}. Logged at: {Placeholder:MMMM dd, yyyy}", user.Id);
            return NotFound("User data not found");
        }
        // log datetime with information
        _logger.LogInformation("Returning user information for user {0}, number of favorites: {1}.  Logged at: {Placeholder:MMMM dd, yyyy}", user.Id, userDto.FavoriteMovies.Count(), DateTimeOffset.UtcNow);
        return Ok(userDto);
    }

    [HttpGet]
    [Route("api/users")]
    public async Task<DataResponse<List<UserEditDto>>> Getusers()
    {
        // add this to the service class
        // add exception handling
        // add logging etc.
        var users = await (from u in _context.Users
                           let query = (from ur in _context.Set<IdentityUserRole<string>>()
                                        where ur.UserId.Equals(u.Id)
                                        join r in _context.Roles on ur.RoleId equals r.Id
                                        select r.Name).ToList()
                           select new UserEditDto
                           {
                                 Id = u.Id,
                                 UserName = u.UserName,
                                 Email = u.Email,
                                 EmailConfirmed = u.EmailConfirmed,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Admin = query.Contains("Admin")
                           }).ToListAsync();

        return new DataResponse<List<UserEditDto>>
        {
            Data = users,
            Succeeded = true,
            Message = "Users retrieved successfully"
        };
    }

    [HttpGet]
    [Route("api/ToggleEnabledUser")]
    public async Task<bool> ToggleEnabledUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;

        }
        user.EmailConfirmed = !user.EmailConfirmed;
        await _userManager.UpdateAsync(user);
        return true;
    }
}
