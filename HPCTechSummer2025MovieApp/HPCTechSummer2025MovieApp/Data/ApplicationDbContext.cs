using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HPCTechSummer2025MovieApp.Model;

namespace HPCTechSummer2025MovieApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ApplicationUserMovie> ApplicationUserMovies { get; set; }
    }


}
