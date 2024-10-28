using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain;
using FilmLib.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace FilmLib.Application.Films.Commands.AddRate;

public class AddRateCommandHandler(AppDbContext context, IDistributedCache cache) : ICommandHandler<AddRateCommand>
{
    public async Task<Result> Handle(AddRateCommand request, CancellationToken cancellationToken)
    {
        var key = $"film-{request.FilmId}";
        var rating = Rating.Create(request.VoteValue, request.FilmId, request.UserId);
        if (rating.IsFailure)
            return Result.Failure(rating.Error);
        
        await context.Ratings.AddAsync(rating.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        // var listOfRatings = await context.Ratings
        //     .Where(r => r.FilmId == request.FilmId)
        //     .ToListAsync(cancellationToken);
        //
        // var result = RatingObject.CalculateRating(listOfRatings);
        // if (result.IsFailure)
        //     return Result.Failure(result.Error);
        //
        // await context.Films
        //     .Where(f => f.Id == request.FilmId)
        //     .ExecuteUpdateAsync(setters =>
        //         setters.SetProperty(
        //             f => f.RatingObject, result.Value), cancellationToken: cancellationToken);
        //
        // await context.SaveChangesAsync(cancellationToken);
        
        // instead of this code, I used the trigger in the database to calculate the rating of the film
        
        await cache.RemoveAsync(key, cancellationToken);
        
        return Result.Success();
    }
}