using Moq;
using HPCTechSummer2025MovieApp.Services;
using HPCTechSummer2025MovieApp.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HPCTechSummer2025MovieAppShared;
using HPCTechSummer2025MovieApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuGet.Frameworks;

namespace HPCTechSummer2025MovieApp.Test;

public class Tests
{
    // mock the MovieService, Logger, UserService

    // the reason you use Interfaces for your service classes is so that
    // you can implement that interface in a MOCK class
    private readonly Mock<IMovieService> _movieServiceMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ILogger<MovieController>> _loggerMock = new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetOMDBMovie_ShouldReturnMovieDto_WhereMovieExists()
    {
        // Arrange
        string omdbId = "tt0133093";
        string userName = "eric.couch@example.net";
        MovieDto movieDto = new MovieDto()
        {
            Title = "The Matrix",
            Year = "1999",
            Rated = "R",
            Released = "31 Mar 1999",
            Runtime = "136 min",
            Genre = "Action, Sci-Fi",
            Director = "Lana Wachowski, Lilly Wachowski",
            Writer = "Lilly Wachowski, Lana Wachowski",
            Actors = "Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss",
            Plot = "When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.",
            Language = "English",
            Country = "United States, Australia",
            Awards = "Won 4 Oscars. 42 wins & 52 nominations total",
            Poster = "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_SX300.jpg",
            Metascore = "73",
            imdbRating = "8.7",
            imdbVotes = "2,179,234",
            imdbID = "tt0133093",
            Type = "movie",
            DVD = "N/A",
            BoxOffice = "$172,076,928",
            Production = "N/A",
            Website = "N/A",
            Response = "True"

        };

        ApplicationUser applicationUser = new ApplicationUser()
        {
            Id = "3ba41060-f562-4d13-a168-10daf4e9c897",
            UserName = userName,
            Email = userName,
            NormalizedEmail = userName.ToUpper(),
            NormalizedUserName = userName.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            PasswordHash = "hashedPassword"
        };
        _movieServiceMock.Setup(x => x.GetOMDBMovie(omdbId))
                        .ReturnsAsync(movieDto);
        _userServiceMock.Setup(x => x.GetUserByUserNameAsync(userName))
                        .ReturnsAsync(applicationUser);

        _loggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));

        MovieController movieController = new MovieController(_loggerMock.Object,
                                                                _movieServiceMock.Object,
                                                                _userServiceMock.Object);

        // Act
        var response = await movieController.GetOMDBMovie(omdbId);
        var okResult = response as OkObjectResult;
        MovieDto movie = okResult?.Value as MovieDto;

        // Assert 
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<OkObjectResult>(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOf<MovieDto>(movie);
        Assert.That(movie.imdbID, Is.EqualTo(omdbId));
        Assert.That(movie.Title, Is.EqualTo("The Matrix"));
        Assert.That(movie.Year, Is.EqualTo("1999"));
        Assert.That(movie.Rated, Is.EqualTo("R"));
    }
}