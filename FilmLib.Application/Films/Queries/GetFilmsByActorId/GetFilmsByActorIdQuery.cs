using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Queries.GetFilmsByActorId;

public record GetFilmsByActorIdQuery(Guid ActorId) : IQuery<List<FilmResponse>>;
