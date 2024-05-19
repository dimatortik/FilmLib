using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;

namespace FilmLib.Application.FilmActions.Queries.GetFilms;

public class GetFilmsQuery : IQuery<List<Film>>;