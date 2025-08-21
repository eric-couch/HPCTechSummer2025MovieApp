using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HPCTechSummer2025MovieApp.Client.Pages;

public partial class Home
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    public HttpClient Http { get; set; }
    public bool IsAuthenticated { get; set; } = false;
    public UserDTO User { get; set; } = new UserDTO();
    protected override async Task OnInitializedAsync()
    {
        // make sure the user is logged in
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity?.IsAuthenticated == true)
        {
            IsAuthenticated = true;
            try
            {
                // go get that users data (UserDto)
                this.User = await Http.GetFromJsonAsync<UserDTO>("api/User");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                // add error handling logic here if needed
                // including logging, etc.
            }

        }






    }
}
