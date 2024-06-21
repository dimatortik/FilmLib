using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.Genre;

public class GenreRequest
{
    [FromForm(Name = "title")]
    public string? Title { get; set; }

    [FromForm(Name = "description")]
    public string? Description { get; set; }
    
}
    