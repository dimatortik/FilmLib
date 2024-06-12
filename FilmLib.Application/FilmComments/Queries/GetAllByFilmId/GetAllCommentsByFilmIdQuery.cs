using FilmLib.Application.Messaging;

namespace FilmLib.Application.FilmComments.Queries.GetAllByFilmId;

public record GetAllCommentsByFilmIdQuery(Guid FilmId) : IQuery<List<FilmCommentResponse>>;
