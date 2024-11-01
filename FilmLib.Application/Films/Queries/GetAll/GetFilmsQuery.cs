using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Queries.GetAll;

public record GetFilmsQuery(
    string? SearchTerm, 
    string? SortColumn, 
    string? SortOrder,
    List<string> Genres,
    int Page,
    int PageSize) : IQuery<PagedList<FilmResponse>>;