using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.Actor;
public class ActorRequest
{
    [FromForm(Name = "actorImage")]
    public IFormFile? ActorImage { get; set; }

    [FromForm(Name = "actorName")]
    public string? ActorName { get; set; }

    [FromForm(Name = "actorDescription")]
    public string? ActorDescription { get; set; }
    
    
}