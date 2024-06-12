using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.Create;

public record CreateFilmCommand(
    string TitleImageLink, 
    string Title, 
    string? Description, 
    int? Year,
    string? Country, 
    string? Director, 
    string FilmVideoLink,
    List<Guid> Actors,
    List<int> Genres) : ICommand;

    