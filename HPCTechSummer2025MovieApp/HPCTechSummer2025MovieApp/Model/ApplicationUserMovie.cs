using HPCTechSummer2025MovieApp.Model;
using HPCTechSummer2025MovieApp.Data;

namespace HPCTechSummer2025MovieApp.Model;

public class ApplicationUserMovie
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public string MovieId { get; set; }
    public Movie Movie { get; set; }
}
