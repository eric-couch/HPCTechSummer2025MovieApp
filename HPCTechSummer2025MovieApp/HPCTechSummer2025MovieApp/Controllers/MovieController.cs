using Microsoft.AspNetCore.Mvc;
using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Identity;
using HPCTechSummer2025MovieApp.Data;
using HPCTechSummer2025MovieApp.Model;
using Microsoft.EntityFrameworkCore;
using HPCTechSummer2025MovieApp.Services;

namespace HPCTechSummer2025MovieApp.Controllers; 

public class MovieController : Controller
{
    private readonly ILogger<MovieController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMovieService _movieService;
    
    public MovieController( ILogger<MovieController> logger,
                            IMovieService movieService,
                            UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _movieService = movieService;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm, int page)
    {
        try
        {
            var searchResult = await _movieService.SearchOMDBMovies(searchTerm, page);

            if (searchResult is null)
            {
                // TODO: return problem detail here
                return NotFound("No movies found for the given search term.");
            }
            else
            {
                DataResponse<MovieSearchResultDto> response = new DataResponse<MovieSearchResultDto>();
                if (Int32.Parse(searchResult.totalResults) == 0)
                {
                    response = new DataResponse<MovieSearchResultDto>
                    {
                        Data = searchResult,
                        Message = "No movies found for that search term",
                        Succeeded = true
                    };
                    return Ok(response);
                }
                response = new DataResponse<MovieSearchResultDto>
                {
                    Data = searchResult,
                    Message = "Movies retrieved successfully",
                    Succeeded = true
                };
                return Ok(response);
            }
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            _logger.LogError(ex, "An error occurred while searching for movies.");
            // return problem detail here instead of generic 500
            return StatusCode(500, $"Internal server error: {ex.Message}");
            
        }
    }

    [HttpGet]
    [Route("api/GetMovie")]
    public async Task<IActionResult> GetOMDBMovie(string imdbId)
    {
        MovieDto searchResult = await _movieService.GetOMDBMovie(imdbId);
        if (searchResult is null)
        {
            // problem detail
            return NotFound("Movie not found for the given IMDb ID.");
        }
        else
        {
            // replace this with DataResponse
            return Ok(searchResult);
        }
    }

    [HttpPost("api/add-movie")]
    public async Task<ActionResult> AddMovie([FromBody] MovieDto moviedto)
    {
        // Check if User.Identity is null
        if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
        {
            var problem = new ProblemDetails
            {
                Title = "Authentication Required",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "User is not authenticated.",
                Instance = HttpContext.Request.Path
            };

            return Unauthorized(problem);
        }
        var userName = User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user is null)
        {
            var problem = new ProblemDetails
            {
                Title = "User Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = "The authenticated user could not be found in the system.",
                Instance = HttpContext.Request.Path
            };

            return NotFound(problem);
        }

        // Check if the movie is already in the user's favorite movies using the database directly


        //if (existingFavorite != null)
        //{
        //    var problem = new ProblemDetails
        //    {
        //        Title = "Movie Already in Favorites",
        //        Status = StatusCodes.Status400BadRequest,
        //        Detail = $"Movie '{existingFavorite.Movie.Title}' is already in the user's favorite movies.",
        //        Instance = HttpContext.Request.Path
        //    };

        //    return BadRequest(problem);


        //var movie = _dbContext.Movies.Find(moviedto.imdbID);
        //var movie = _movieService.GetOMDBMovie(moviedto.imdbID);

        Movie? movie = await _movieService.FindMovie(moviedto.imdbID);
        if (movie is null)
        {
            moviedto = await _movieService.GetOMDBMovie(moviedto.imdbID);
            Movie newMovie = new Movie
            {
                imdbID = moviedto.imdbID,
                Title = moviedto.Title,
                Year = moviedto.Year,
                Rated = moviedto.Rated,
                Released = moviedto.Released,
                Runtime = moviedto.Runtime,
                Genre = moviedto.Genre,
                Director = moviedto.Director,
                Writer = moviedto.Writer,
                Actors = moviedto.Actors,
                Plot = moviedto.Plot,
                Language = moviedto.Language,
                Country = moviedto.Country,
                Awards = moviedto.Awards,
                Poster = moviedto.Poster,
                Metascore = moviedto.Metascore,
                imdbRating = moviedto.imdbRating,
                imdbVotes = moviedto.imdbVotes,
                Type = moviedto.Type,
                DVD = moviedto.DVD,
                BoxOffice = moviedto.BoxOffice,
                Production = moviedto.Production,
                Website = moviedto.Website,
                Response = moviedto.Response
            };
            movie = await _movieService.AddMovie(newMovie);
        }

        // Add the movie to the user's favorite movies
        var applicationUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = user.Id,
            MovieId = movie.imdbID
        };

        await _movieService.AddMovieToUser(applicationUserMovie);
        
        Response res = new Response
        {
            Succeeded = true,
            Message = $"Movie '{movie.Title}' added to favorites."
        };
        return Ok(res);
    }

}
