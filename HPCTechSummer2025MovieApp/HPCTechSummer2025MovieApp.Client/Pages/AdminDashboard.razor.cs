using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using System.Net.Http;
using System.Net.Http.Json;

namespace HPCTechSummer2025MovieApp.Client.Pages;

public partial class AdminDashboard
{
    [Inject]
    public HttpClient Http { get; set; }
    public List<UserEditDto> Users { get; set; } = new List<UserEditDto>();
    public SfGrid<UserEditDto> userGrid { get; set; }

    private SfToast ToastObj;
    private string toastContent = string.Empty;
    private string toastCss = "e-toast-success";
    

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Use relative URL - works in all environments
            var res = await Http.GetFromJsonAsync<DataResponse<List<UserEditDto>>>("api/users");
            if (res?.Succeeded == true)
            {
                Users = res.Data;
            }
            else
            {
                toastContent = res?.Message ?? "Failed to load users";
                toastCss = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100);
                if (ToastObj != null)
                    await ToastObj.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            toastContent = $"Error loading users: {ex.Message}";
            toastCss = "e-toast-danger";
            StateHasChanged();
            await Task.Delay(100);
            if (ToastObj != null)
                await ToastObj.ShowAsync();
        }
    }

    public async Task UserDoubleClickHandler(RecordDoubleClickEventArgs<UserEditDto> args)
    {
        return;
    }

    public async Task ToggleEnabledUser(ChangeEventArgs args, string userId)
    {
        try
        {
            bool res = await Http.GetFromJsonAsync<bool>($"api/ToggleEnabledUser?userId={Uri.EscapeDataString(userId)}");
            if (!res)
            {
                toastContent = "Toggle of User Enabled Failed!";
                toastCss = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100);
                if (ToastObj != null)
                    await ToastObj.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            toastContent = $"Error toggling user: {ex.Message}";
            toastCss = "e-toast-danger";
            StateHasChanged();
            await Task.Delay(100);
            if (ToastObj != null)
                await ToastObj.ShowAsync();
        }
    }

}
