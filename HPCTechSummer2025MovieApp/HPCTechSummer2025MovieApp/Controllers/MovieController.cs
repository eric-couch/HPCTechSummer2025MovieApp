using Microsoft.AspNetCore.Mvc;
using HPCTechSummer2025MovieAppShared;

namespace HPCTechSummer2025MovieApp.Controllers; 

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MovieController> _logger;
    private readonly IConfiguration _config;

    private readonly string OMDBBaseUrl = "https://www.omdbapi.com/";
    

    public MovieController( HttpClient httpClient, 
                            ILogger<MovieController> logger,
                            IConfiguration config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _config = config;
    }

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm)
    {
        try
        {
            string OMDBAPIKey = _config["Movies:OMDBApiKey"];
            string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&s={searchTerm}";
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
}
