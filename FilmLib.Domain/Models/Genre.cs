using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;

namespace FilmLib.Domain.Models;


public class Genre
{
    public const int MAX_GENRE_DESCRIPTION_LENGTH = 1500; 
    public const int MAX_GENRE_TITLE_LENGTH = 20;

    private readonly List<Film> _films = new ();
    private Genre(string title, string description )
    {
        Title = title;
        Description = description;
    }
    
    public int Id { get; private set; }

    public string Title  { get; private set; }

    public string Description { get; private set; }

    public IReadOnlyCollection<Film> Films => _films;

    public void AddFilm(Film film) => _films.Add(film);
    

    public static Result<Genre> Create(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_GENRE_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Genre>(DomainException.EmptyOrOutOfRange("description").Message);
        }
        
        if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_GENRE_TITLE_LENGTH)
        {
            return Result.Failure<Genre>(DomainException.EmptyOrOutOfRange("Title").Message);
        }
        
        return Result.Success<Genre>(new (title, description));
    }
    
    public Result Edit(string? title, string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > MAX_GENRE_DESCRIPTION_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange("Description").Message);
        }
        
        if (!string.IsNullOrWhiteSpace(title) && title.Length > MAX_GENRE_TITLE_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange("Title").Message);
        }
        
        Title = title ?? Title;
        Description = description ?? Description;
        return Result.Success();
    }
    
}
