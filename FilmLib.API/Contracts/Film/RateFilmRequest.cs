using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.Film;

public class RateFilmRequest
{
    [FromForm(Name = "ratingNumber")]
    public decimal RatingValue { get; set; }
    [FromForm(Name = "userId")]
    public Guid UserId { get; set; }
    
}