using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Queries.GetAll;

public record GetAllGenresQuery(
    string SearchTerm) : IQuery<List<GenreResponse>>;
