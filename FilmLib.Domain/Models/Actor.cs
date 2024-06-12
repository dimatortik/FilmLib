using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;
using FilmLib.Domain.ValueObjects;

namespace FilmLib.Domain.Models;

public class Actor
{
    public const int MAX_ACTOR_NAME_LENGTH = 25;
    public const int MAX_ACTOR_DESCRIPTION_LENGTH = 1000;
    
    private readonly List<Film> _films = new ();
    
    private Actor(string actorImageLink, string name, string description)
    {
        Id = Guid.NewGuid();
        ActorImageLink = actorImageLink;
        Name = name;
        Description = description;
        Rating = Rating.Create().Value;
    }
    public Guid Id { get; private set; }
    
    public string ActorImageLink { get; private set; }

    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Rating Rating { get; private set; }

    public IReadOnlyCollection<Film> Films => _films;
    
    public void AddFilm(Film film) =>_films.Add(film);
    
    public static Result<Actor> Create(
        string actorImageLink, 
        string name, 
        string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_ACTOR_NAME_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(name).Message);
        }
        
        if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_ACTOR_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(description).Message);
        }

        var actor = new Actor(
            actorImageLink, 
            name,
            description);

        return Result.Success(actor);
    }
    
    public Result<Actor> Edit(
        string? actorImageLink, 
        string? name, 
        string? description)
    {
        if (name != null && name.Length > MAX_ACTOR_NAME_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(name).Message);
        }
        
        if (description != null && description.Length > MAX_ACTOR_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Actor>(DomainException.EmptyOrOutOfRange(description).Message);
        }

        ActorImageLink = actorImageLink ?? ActorImageLink;
        Name = name ?? Name;
        Description = description ?? Description;

        return Result.Success(this);
    }
    
    public Result<Rating> AddVote(int ratingValue)
    {
        Rating = Rating.AddVote(ratingValue).Value;
        return Rating;
    }
    
}