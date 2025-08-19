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
            return NotFound("User data not found");
        }
        return Ok(userDto);
    }

}
