using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Queries.GetGenresByFilmId;

public record GetGenresByFilmIdQuery(Guid FilmId) : IQuery<List<GenreResponse>>;
