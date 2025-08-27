using HPCTechSummer2025MovieApp.Data;
using HPCTechSummer2025MovieApp.Model;
using HPCTechSummer2025MovieAppShared;
using Microsoft.EntityFrameworkCore;

namespace HPCTechSummer2025MovieApp.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<MovieService> _logger;
    private readonly ApplicationDbContext _context;

    private readonly string OMDBBaseUrl = "https://www.omdbapi.com/";

    public MovieService(    HttpClient httpClient,
                            IConfiguration config,
                            ILogger<MovieService> logger,
                            ApplicationDbContext dbContext)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
        _context = dbContext;
    }

    public async Task<MovieSearchResultDto>? SearchOMDBMovies(string searchTerm, int page)
    {
        string OMDBAPIKey = _config["Movies:OMDBApiKey"];
        string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&s={searchTerm}&page={page}";
        _logger.LogInformation($"Searching for movies with term: {searchTerm} using URL: {query}");
        var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>(query);
        return searchResult;
    }

    public async Task<MovieDto>? GetOMDBMovie(string imdbId)
    {
        try
        {
            string OMDBAPIKey = _config["Movies:OMDBApiKey"];
            string query = $"{OMDBBaseUrl}?apikey={OMDBAPIKey}&i={imdbId}";
            _logger.LogInformation($"Fetching movie details for IMDb ID: {imdbId} using URL: {query}");
            var searchResult = await _httpClient.GetFromJsonAsync<MovieDto>(query);
            return searchResult;
        } catch
        {
            return null;
        }
    }

    public async Task<bool> AddMovieToUser(ApplicationUserMovie applicationUserMovie)
    {
        var existingFavorite = await _context.ApplicationUserMovies
            .Include(aum => aum.Movie)
            .FirstOrDefaultAsync(aum => aum.ApplicationUserId == applicationUserMovie.ApplicationUserId && aum.MovieId == applicationUserMovie.MovieId);

        if (existingFavorite is null)
        {
            // TODO: add try catch
            await _context.ApplicationUserMovies.AddAsync(applicationUserMovie);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;

    }

    public async Task<Movie>? FindMovie(string imdbId)
    {
        return await _context.Movies.FindAsync(imdbId);
    }

    public async Task<Movie> AddMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        _context.SaveChangesAsync();
        return movie;
    }
}
