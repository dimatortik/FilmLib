namespace FilmLib.API.Contracts;

public record RegisterUser(
    string Email,
    string Password,
    string Username
);