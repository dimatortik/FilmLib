using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;


namespace FilmLib.Domain.Models;

public class FilmComment
{
    public const int MAX_FILM_COMMENT_LENGTH = 500;
    private FilmComment(string body, Guid userId, Guid filmId)
    {
        Id = Guid.NewGuid();
        Body = body;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        FilmId = filmId;
    }

    public Guid Id { get; private set; }
    
    public string Body { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
    
    public Guid FilmId { get; set; }
    
    public virtual Film Film { get; set; }
    
    public static Result<FilmComment> Create(string body, Guid userId, Guid filmId)
    {
        
        if ( string.IsNullOrWhiteSpace(body) || body.Length > MAX_FILM_COMMENT_LENGTH)
        {
            return Result.Failure<FilmComment>(DomainException.EmptyOrOutOfRange("Text of comment").Message);
        }
        
        return Result.Success(new FilmComment(body, userId, filmId));
    }

    
}