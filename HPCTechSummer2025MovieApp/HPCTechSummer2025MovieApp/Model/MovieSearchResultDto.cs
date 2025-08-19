namespace HPCTechSummer2025MovieApp.Model;

public class MovieSearchResultDto
{
    public List<MovieSearchResultItem> Search { get; set; }
    public string totalResults { get; set; }
    public string Response { get; set; }

}
