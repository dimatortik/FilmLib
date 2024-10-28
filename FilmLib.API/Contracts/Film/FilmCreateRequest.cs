using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.Film;

public class FilmCreateRequest
{
    [FromForm(Name = "titleImage")]
    public IFormFile? TitleImage { get; set; }

    [FromForm(Name = "title")]
    public string? Title { get; set; }

    [FromForm(Name = "description")]
    public string? Description { get; set; }

    [FromForm(Name = "year")]
    public int? Year { get; set; }

    [FromForm(Name = "country")]
    public string? Country { get; set; }

    [FromForm(Name = "director")]
    public string? Director { get; set; }

    [FromForm(Name = "filmVideo")]
    public IFormFile? FilmVideo { get; set; }

    [FromForm(Name = "actors")]
    public List<Guid> Actors { get; set; }

    [FromForm(Name = "genres")]
    public List<int> Genres { get; set; }
    
}
    