namespace FilmLib.API.Contracts;

public record FilmDto(
    string TitleImageLink,
    string Title,
    string? Description,
    int? Year,
    string? Country,
    string? Director,
    string FilmVideoLink);
