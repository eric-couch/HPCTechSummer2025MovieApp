using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using HPCTechSummer2025MovieAppShared;
using Syncfusion.Blazor.Grids;

namespace HPCTechSummer2025MovieApp.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; }
    private string searchTerm = string.Empty;
    private List<MovieSearchResultItem> searchResult;
    private int totalItems = 0;
    private MovieSearchResultItem selectedMovie;
    private MovieDto? omdbMovie { get; set; } = null;

    private async Task SearchMovies()
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            try
            {
                var res = await Http.GetFromJsonAsync<MovieSearchResultDto>($"api/SearchMovies?searchTerm={Uri.EscapeDataString(searchTerm)}");
                if (res is not null && res.Response == "True")
                {
                    searchResult = res.Search;
                    totalItems = Int32.Parse(res.totalResults);
                    // Optionally, you can also handle the case where no results are found
                    if (totalItems == 0)
                    {
                        Console.WriteLine("No movies found.");
                    }
                }
                else
                {
                    Console.WriteLine("No movies found or an error occurred.");
                }

                if (searchResult != null)
                {
                    // Handle the search result, e.g., display it in the UI
                    Console.WriteLine($"Found {searchResult.Count} movies for '{searchTerm}'");
                }
                else
                {
                    Console.WriteLine("No movies found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for movies: {ex.Message}");
            }
        }
    }

    public async Task GetSelectRows(RowSelectEventArgs<MovieSearchResultItem> args)
    {
        selectedMovie = args.Data;
        omdbMovie = await Http.GetFromJsonAsync<MovieDto>($"api/GetMovie?imdbId={selectedMovie.imdbID}");
    }

}
