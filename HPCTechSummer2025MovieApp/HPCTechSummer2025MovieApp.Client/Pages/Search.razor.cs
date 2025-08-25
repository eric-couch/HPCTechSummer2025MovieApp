using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using HPCTechSummer2025MovieAppShared;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

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
    private SfPager Page;
    private int page { get; set; } = 1;

    private async Task SearchMovies()
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            try
            {
                var res = await Http.GetFromJsonAsync<MovieSearchResultDto>($"api/SearchMovies?searchTerm={Uri.EscapeDataString(searchTerm)}&page={page}");
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

    public async Task PageClick(PagerItemClickEventArgs args)
    {
        page = args.CurrentPage;
        await SearchMovies();
    }

    public async Task ToolBarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "AddMovie")
        {
            await AddMovie();
        }
    }

    public async Task AddMovie()
    {
        if (selectedMovie is null)
        {
            // No movie selected, handle accordingly
            // add toast component
            return;
        }
        MovieDto newMovie = new MovieDto
        {
            imdbID = selectedMovie.imdbID
        };

        var response = await Http.PostAsJsonAsync("api/add-movie", newMovie);
        if (response.IsSuccessStatusCode)
        {
            // Movie added successfully
            Console.WriteLine("Movie added to favorites.");
            // Optionally, you can show a success message to the user
        }
        else
        {
            // Handle error response
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error adding movie: {errorMessage}");
            // Optionally, you can show an error message to the user
        }
    }

}
