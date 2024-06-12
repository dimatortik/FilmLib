using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;

namespace FilmLib.Domain.ValueObjects;

public class Rating
{
    private double _ratingValue;
    private int _numberOfVotes;

    private Rating(double ratingValue, int numberOfVotes)
    {
        _ratingValue = ratingValue;
        _numberOfVotes = numberOfVotes;
    }

    public double RatingValue
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
    
    public static Result<Rating> Create()
    {
        return new Rating(0, 0);
    }
    

    public Result<Rating> AddVote(double newVote)
    {
        if (newVote < 0 || newVote > 10)
            Result.Failure(DomainException.ValueObjects.RatingEmptyOrOutOfRage().Message);
        
        RatingValue = (RatingValue * NumberOfVotes + newVote) / (NumberOfVotes + 1);
       NumberOfVotes++;
        return Result.Success(this);
    }
}
