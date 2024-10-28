using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;

namespace FilmLib.Domain.ValueObjects;

public class RatingObject
{
    private decimal _ratingValue;
    private int _numberOfVotes;

    private RatingObject(decimal ratingValue, int numberOfVotes)
    {
        _ratingValue = ratingValue;
        _numberOfVotes = numberOfVotes;
    }

    public decimal RatingValue
    {
        get => _ratingValue;
        private set
        {
            if (value < 0 || value > 10)
                Result.Failure(DomainException.ValueObjects.RatingEmptyOrOutOfRage().Message);
            _ratingValue = value;
        }
    }

    public int NumberOfVotes
    {
        get => _numberOfVotes;
        private set
        {
            if (value < 0)
                Result.Failure(DomainException.ValueObjects.RatingEmptyOrOutOfRage().Message);

            _numberOfVotes = value;
        }
    }
    
    public static Result<RatingObject> Create()
    {
        return new RatingObject(0, 0);
    }
    
    public static Result<RatingObject> Create(decimal ratingValue, int numberOfVotes)
    {
        return new RatingObject(ratingValue, numberOfVotes);
    }
    

    public Result<RatingObject> AddRate(decimal newVote)
    {
        if (newVote < 0 || newVote > 10)
            Result.Failure(DomainException.ValueObjects.RatingEmptyOrOutOfRage().Message);
        
        RatingValue = (RatingValue * NumberOfVotes + newVote) / (NumberOfVotes + 1);
       NumberOfVotes++;
        return Result.Success(this);
    }
    
    public static Result<RatingObject> CalculateRating(List<Rating> ratings)
    {
        if (ratings.Count == 0)
        {
            return Result.Failure<RatingObject>(DomainException.EmptyOrOutOfRange("Rating").Message);
        }

        var ratingValue = ratings.Select(x => x.RatingNumber).Average();
        
        var ratingObject = RatingObject.Create(ratingValue, ratings.Count);
        
        if (ratingObject.IsFailure)
            return Result.Failure<RatingObject>(ratingObject.Error);
        
        return Result.Success(ratingObject.Value);
    }

}
