using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;
using FilmLib.Domain.ValueObjects;

namespace FilmLib.Domain.Models;

public class Film
{
    private readonly List<FilmComment> _filmComments = [];
    private readonly List<Genre> _genres = new();
    private readonly List<Actor> _actors = new();
    private Film(string titleImageLink, string title, string? description, int? year,
        string? country, string? director, string filmVideoLink)
    {
        Id = Guid.NewGuid();
        TitleImageLink = titleImageLink;
        Title = title;
        Description = description;
        Year = year;
        Views = 0;
        Rating = Rating.Create().Value;
        Country = country;
        Director = director;
        FilmVideoLink = filmVideoLink;
    }
    public const int MAX_FILM_TITLE_LENGTH = 50;
    public const int MAX_FILM_DESCRIPTION_LENGTH = 1500;
    
    public Guid Id { get; private set; }
    public string TitleImageLink { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public int Views { get; private set; }
    public Rating Rating { get; set; }
    public int? Year{ get; private set; }
    public IReadOnlyCollection<Genre> Genres  => _genres;
    public string? Country { get; private set;}
    public string? Director { get; private set;}
    public IReadOnlyCollection<Actor> Actors => _actors;
    public string FilmVideoLink { get; private set; }
    public IReadOnlyCollection<FilmComment> FilmComments => _filmComments;
    
    public void AddActors(List<Actor> actors) => _actors.AddRange(actors);
    
    public void AddGenres(List<Genre> genres) => _genres.AddRange(genres);
    
    public int CountView() => Views++;

    public static Result<Film> Create(
        string titleImageLink, 
        string title, 
        string? description, 
        int? year,
        string? country, 
        string? director, 
        string filmVideoLink)
    {
        if (string.IsNullOrWhiteSpace(titleImageLink))
        {
            return Result.Failure<Film>(
                DomainException.EmptyOrOutOfRange(nameof(titleImageLink)).Message);
        }

        if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_FILM_TITLE_LENGTH)
        {
            return Result.Failure<Film>(
                DomainException.EmptyOrOutOfRange(nameof(title)).Message);
        }

        if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_FILM_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Film>(
                DomainException.EmptyOrOutOfRange(nameof(description)).Message);
        }

        var film = new Film(titleImageLink, 
            title, 
            description, 
            year, 
            country, 
            director, 
            filmVideoLink);
        
        return Result.Success(film);
    }

    
    public Result AddComment(string body, Guid userId)
    {
        var comment = FilmComment.Create(body, userId, Id);
        if (comment.IsFailure)
        {
            return Result.Failure(comment.Error);
        }
        _filmComments.Add(comment.Value);
        return Result.Success();
    }
    
    public Result Edit(
        string? titleImageLink, 
        string? title, 
        string? description, 
        int? year, 
        string? country, 
        string? director, 
        string? filmVideoLink)
    {
        
        if (!string.IsNullOrWhiteSpace(title) && title.Length > MAX_FILM_TITLE_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange(nameof(title)).Message);
        }
        
        if (!string.IsNullOrWhiteSpace(description) && description.Length > MAX_FILM_DESCRIPTION_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange(nameof(description)).Message);
        }

        if (year != 0 && year < 1800)
        {
            return Result.Failure(DomainException.OutOfRange(nameof(year)).Message);
        }

        TitleImageLink = titleImageLink == string.Empty? TitleImageLink : titleImageLink;
        Title = title ?? Title;
        Description = description ?? Description;
        Year = year ?? Year;
        Country = country ?? Country;
        Director = director ?? Director;
        FilmVideoLink = filmVideoLink == string.Empty? FilmVideoLink : filmVideoLink;
        return Result.Success();
    } 
    
    public Result AddVote(int ratingValue)
    {
        var result = Rating.AddVote(ratingValue);
        if (result.IsFailure)
        {
            return result;
        }
        Rating = result.Value;
        return Result.Success();
    }
    
    
}
