namespace HPCTechSummer2025MovieAppShared;

public class UserDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<MovieDto> FavoriteMovies { get; set; } = new List<MovieDto>();
}
