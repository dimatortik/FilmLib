using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;

namespace FilmLib.Domain.Models;

public class Actor
{
    public const int MAX_ACTOR_NAME_LENGTH = 25;
    public const int MAX_ACTOR_DESCRIPTION_LENGTH = 500;
    
    private readonly List<Film> _films = new ();
    
    private Actor()
    {
    }
    public Guid Id { get; private set; }
    
    public string ActorImageLink { get; private set; }

    public string Name { get; private set; }
    
    public string Description { get; private set; }

    public IReadOnlyCollection<Film> Films => _films;
    
    public void AddFilm(Film film) =>_films.Add(film);
    
    public static Result<Actor> Create(Guid id, string actorImageLink, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_ACTOR_NAME_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(name).Message);
        }
        
        if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_ACTOR_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(description).Message);
        }
        
        var actor = new Actor
        {
            Id = Guid.NewGuid(),
            ActorImageLink = actorImageLink,
            Name = name,
            Description = description
        };
        return Result.Success(actor);
    }
    
}