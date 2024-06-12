using FilmLib.Domain.ValueObjects;

namespace FilmLib.Application.Actors.Queries;

public class ActorResponse{
    public Guid Id { get; set; }
    public string ActorImageLink { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    
}