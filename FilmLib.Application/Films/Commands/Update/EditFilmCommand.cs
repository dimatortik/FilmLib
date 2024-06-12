using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.Update;

public record EditFilmCommand(
    Guid Id,
    string TitleImageLink,
    string Title,
    string? Description,
    int? Year,
    string? Country,
    string? Director,
    string FilmVideoLink) : ICommand;
