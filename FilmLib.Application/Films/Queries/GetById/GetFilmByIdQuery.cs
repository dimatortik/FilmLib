using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;

namespace FilmLib.Application.Films.Queries.GetById;

public sealed record GetFilmByIdQuery(Guid Id) : IQuery<FilmResponse>
{
    
}