using Microsoft.AspNetCore.Mvc;
using HPCTechSummer2025MovieAppShared;

namespace HPCTechSummer2025MovieApp.Controllers; 

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string OMDBAPIKey = "apikey=86c39163";
    private readonly string OMDBBaseUrl = "https://www.omdbapi.com/";
    private readonly ILogger<MovieController> _logger;

    public MovieController(HttpClient httpClient, ILogger<MovieController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm)
    {
        try
        {
            string query = $"{OMDBBaseUrl}?{OMDBAPIKey}&s={searchTerm}";
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
}
