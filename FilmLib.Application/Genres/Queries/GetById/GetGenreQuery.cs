using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Queries.GetById;

public record GetGenreQuery(int Id) : IQuery<GenreResponse>;
