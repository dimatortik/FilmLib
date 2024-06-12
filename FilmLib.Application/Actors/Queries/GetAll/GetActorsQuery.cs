using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;

namespace FilmLib.Application.Actors.Queries.GetAll;

public record GetActorsQuery(string? SearchTerm) : IQuery<List<ActorResponse>>;