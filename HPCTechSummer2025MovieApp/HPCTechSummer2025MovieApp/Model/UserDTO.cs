namespace HPCTechSummer2025MovieApp.Model;

public class UserDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
}
