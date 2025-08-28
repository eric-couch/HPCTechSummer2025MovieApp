using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using HPCTechSummer2025MovieAppShared;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;

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
    private SfToast ToastObj;
    private string toastContent = string.Empty;
    private string toastCss = "e-toast-success";
    private int page { get; set; } = 1;

    private async Task SearchMovies()
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            try
            {
                var res = await Http.GetFromJsonAsync<DataResponse<MovieSearchResultDto>>($"api/SearchMovies?searchTerm={Uri.EscapeDataString(searchTerm)}&page={page}");
                if (res.Succeeded)
                {
                    searchResult = res.Data.Search;
                    totalItems = Int32.Parse(res.Data.totalResults);
                    // Optionally, you can also handle the case where no results are found
                    if (totalItems == 0)
                    {
                        toastContent = res.Message;
                        toastCss = "e-toast-warning";
                        StateHasChanged();
                        await Task.Delay(100); // Ensure the state is updated before showing the toast
                        await ToastObj.ShowAsync();
                        return;
                    }
                }
                else
                {
                    toastContent = res.Message;
                    toastCss = "e-toast-warning";
                    StateHasChanged();
                    await Task.Delay(100); // Ensure the state is updated before showing the toast
                    await ToastObj.ShowAsync();
                    return;
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
        try
        {
            if (selectedMovie is null)
            {
                toastContent = "No movie selected";
                toastCss = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100); // Ensure the state is updated before showing the toast
                await ToastObj.ShowAsync();
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
                toastContent = $"Added {selectedMovie.Title} to user favorites";
                toastCss = "e-toast-success";
                StateHasChanged();
                await Task.Delay(100); // Ensure the state is updated before showing the toast
                await ToastObj.ShowAsync();
                return;
            }
            else
            {
                // Handle error response
                var problem = await response.Content.ReadFromJsonAsync<ProblemResponse>();
                Console.WriteLine($"Error adding movie: {problem?.Detail ?? "error"}  ");
                toastContent = $"{problem?.Detail ?? "error"}";
                toastCss = "e-toast-danger";
                StateHasChanged();
                await Task.Delay(100); // Ensure the state is updated before showing the toast
                await ToastObj.ShowAsync();
                return;
            }
        } catch (Exception ex)
        {
            Console.WriteLine("Error in AddMovie");
        }
       
    }

}
