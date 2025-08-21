using HPCTechSummer2025MovieAppShared;
using Microsoft.AspNetCore.Components;

namespace HPCTechSummer2025MovieApp.Client.Components;

public partial class MovieDetails
{
    [Parameter]
    public MovieDto? Movie { get; set; } = new MovieDto();
}
