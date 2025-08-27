using HPCTechSummer2025MovieApp.Model;
using HPCTechSummer2025MovieAppShared;

namespace HPCTechSummer2025MovieApp.Services;

public interface IMovieService
{
    Task<MovieSearchResultDto>? SearchOMDBMovies(string searchTerm, int page);
    Task<MovieDto>? GetOMDBMovie(string imdbId);
    Task<bool> AddMovieToUser(ApplicationUserMovie applicationUserMovie);
    Task<Movie> AddMovie(Movie movie);
    Task<Movie>? FindMovie(string imdbId);
}
