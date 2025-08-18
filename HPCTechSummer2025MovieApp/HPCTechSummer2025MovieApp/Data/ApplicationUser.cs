using Microsoft.AspNetCore.Identity;
using HPCTechSummer2025MovieApp.Model;

namespace HPCTechSummer2025MovieApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
        public List<ApplicationUserMovie> FavoriteMovies { get; set; } = new List<ApplicationUserMovie>();
    }

}
