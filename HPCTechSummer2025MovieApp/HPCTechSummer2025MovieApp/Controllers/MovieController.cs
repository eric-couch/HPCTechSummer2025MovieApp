using Microsoft.AspNetCore.Mvc;
using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Identity;
using HPCTechSummer2025MovieApp.Data;
using HPCTechSummer2025MovieApp.Model;

namespace HPCTechSummer2025MovieApp.Controllers; 

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MovieController> _logger;
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    private readonly string OMDBBaseUrl = "https://www.omdbapi.com/";
    

    public MovieController( HttpClient httpClient, 
                            ILogger<MovieController> logger,
                            IConfiguration config,
                            UserManager<ApplicationUser> userManager,
                            ApplicationDbContext dbContext)
    {
        _httpClient = httpClient;
        _logger = logger;
        _config = config;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm, int page)
    {
        try
        {
            string OMDBAPIKey = _config["Movies:OMDBApiKey"];
            string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&s={searchTerm}&page={page}";
            _logger.LogInformation($"Searching for movies with term: {searchTerm} using URL: {query}");
            var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>(query);

            if (searchResult is null)
            {
                return NotFound("No movies found for the given search term.");
            }
            else
            {
                return Ok(searchResult);
            }
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            _logger.LogError(ex, "An error occurred while searching for movies.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("api/GetMovie")]
    public async Task<IActionResult> GetOMDBMovie(string imdbId)
    {
        string OMDBAPIKey = _config["Movies:OMDBApiKey"];
        string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&i={imdbId}";
        _logger.LogInformation($"Fetching movie details for IMDb ID: {imdbId} using URL: {query}");
        var searchResult = await _httpClient.GetFromJsonAsync<MovieDto>(query);
        if (searchResult is null)
        {
            return NotFound("Movie not found for the given IMDb ID.");
        }
        else
        {
            return Ok(searchResult);
        }
    }

    //api/add-movie
    [HttpPost]
    [Route("api/add-movie")]
    public async Task<IActionResult> AddMovie([FromBody] MovieDto moviedto)
    {
        // get user and see if they already have that movie as their favorite
        // if they don't, get the movie info from ombd and add it to their favorites
        MovieDto newMovie = new MovieDto();
        var userName = User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            return NotFound();
        }

        var movie = _dbContext.Movies.Find(moviedto.imdbID);
        if (movie is null)
        {
            string OMDBAPIKey = _config["Movies:OMDBApiKey"];
            string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&i={moviedto.imdbID}";
            _logger.LogInformation($"Fetching movie details for IMDb ID: {moviedto.imdbID} using URL: {query}");
            newMovie = await _httpClient.GetFromJsonAsync<MovieDto>(query);
            if (newMovie is null)
            {
                return NotFound("Movie not found for the given IMDb ID.");
            }
            await _dbContext.Movies.AddAsync(new Movie
            {
                imdbID = newMovie.imdbID,
                Title = newMovie.Title,
                Year = newMovie.Year,
                Rated = newMovie.Rated,
                Released = newMovie.Released,
                Runtime = newMovie.Runtime,
                Genre = newMovie.Genre,
                Director = newMovie.Director,
                Writer = newMovie.Writer,
                Actors = newMovie.Actors,
                Plot = newMovie.Plot,
                Language = newMovie.Language,
                Country = newMovie.Country,
                Awards = newMovie.Awards,
                Poster = newMovie.Poster,
                Metascore = newMovie.Metascore,
                imdbRating = newMovie.imdbRating,
                imdbVotes = newMovie.imdbVotes,
                Type = newMovie.Type,
                DVD = newMovie.DVD,
                BoxOffice = newMovie.BoxOffice,
                Production = newMovie.Production,
                Website = newMovie.Website,
                Response = newMovie.Response
            });
            newMovie = await _httpClient.GetFromJsonAsync<MovieDto>(query);
        }
        
        

        var applicationUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = user.Id,
            MovieId = newMovie.imdbID
        };

        if (user.FavoriteMovies.Any(m => m.MovieId == newMovie.imdbID))
        {
            return BadRequest("Movie already in favorites.");
        }
        user.FavoriteMovies.Add(applicationUserMovie);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
