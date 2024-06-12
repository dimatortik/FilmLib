using FilmLib.Application.Messaging;

namespace FilmLib.Application.Actors.Queries.GetById;

public record GetActorByIdQuery(Guid Id) : IQuery<ActorResponse>;
