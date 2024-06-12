using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.FilmComments.Queries.GetAllByFilmId;

public class GetAllCommentsByFilmIdQueryHandler(AppDbContext context) : IQueryHandler<GetAllCommentsByFilmIdQuery, List<FilmCommentResponse>>
{
    public async Task<Result<List<FilmCommentResponse>>> Handle(GetAllCommentsByFilmIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await context.Comments
            .AsNoTracking()
            .Include(c => c.User)
            .Where(c => c.FilmId == request.FilmId)
            .Select(c => new FilmCommentResponse
            {
                Id = c.Id,
                Body = c.Body,
                UserName = c.User.Username,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return Result.Success(comments);
    }
}