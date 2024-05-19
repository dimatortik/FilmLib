
using FilmLib.Application.Messaging;

namespace FilmLib.Application.FilmActions.Commands.CreateFilm;

public sealed record CreateFilmCommand(
    string TitleImageLink, 
    string Title, 
    string? Description, 
    int? Year,
    string? Country, 
    string? Director, 
    string FilmVideoLink) : ICommand;

    