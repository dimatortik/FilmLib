using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Films.Commands.UpdateRate;

public class UpdateRateCommandHandler(AppDbContext context) : ICommandHandler<UpdateRateCommand>
{
    public async Task<Result> Handle(UpdateRateCommand request, CancellationToken cancellationToken)
    {
        await context.Ratings
            .Where(r => r.FilmId == request.Id && r.UserId == request.UserId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    r => r.RatingNumber, request.RateValue), cancellationToken: cancellationToken);
        
        // var listOfRatings = await context.Ratings
        //     .Where(r => r.FilmId == request.Id)
        //     .ToListAsync(cancellationToken);
        //
        // var result = RatingObject.CalculateRating(listOfRatings);
        // if (result.IsFailure)
        //     return Result.Failure(result.Error);
        // await context.Films
        //     .Where(f => f.Id == request.Id)
        //     .ExecuteUpdateAsync(setters =>
        //         setters.SetProperty(
        //             f => f.RatingObject, result.Value), cancellationToken: cancellationToken);
        
        // instead of this code, I used the trigger in the database to calculate the rating of the film
            
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();


    }
}