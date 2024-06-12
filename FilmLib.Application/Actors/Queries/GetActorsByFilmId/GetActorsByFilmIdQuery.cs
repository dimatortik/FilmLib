using FilmLib.Application.Messaging;

namespace FilmLib.Application.Actors.Queries.GetActorsByFilmId;

public record GetActorsByFilmIdQuery(Guid FilmId) : IQuery<List<ActorResponse>>;
