using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;
using FilmLib.Domain.Models;

namespace FilmLib.Domain;

public class Rating
{
    private Rating(decimal ratingNumber, Guid filmId, Guid userId)
    {
        Id = Guid.NewGuid();
        RatingNumber = ratingNumber;
        FilmId = filmId;
        UserId = userId;
        Date = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }

    public decimal RatingNumber { get; private set; }

    public Guid FilmId { get; private set; }
    
    
    public Film Film { get; private set; }
    
    public Guid UserId { get; private set; }
    
    
    public User User { get; private set; }
    
    public DateTime Date { get; private set; }
    
    
    public static Result<Rating> Create(decimal ratingNumber, Guid filmId, Guid userId)
    {
        if (ratingNumber < 0 || ratingNumber > 10)
        {
            return Result.Failure<Rating>(DomainException.EmptyOrOutOfRange("Rating").Message);
        }

        return Result.Success(new Rating(ratingNumber, filmId, userId));
    }
    
    public Result<Rating> Update(decimal ratingNumber)
    {
        if (ratingNumber < 0 || ratingNumber > 10)
        {
            return Result.Failure<Rating>(DomainException.EmptyOrOutOfRange("Rating").Message);
        }

        RatingNumber = ratingNumber;
        Date = DateTime.UtcNow;
        return Result.Success(this);
    }
    
    

}