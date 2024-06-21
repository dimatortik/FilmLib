using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Queries.GetFilmsByGenreId;

public record GetFilmsByGenreIdQuery(int Id) : IQuery<List<FilmResponse>>;
