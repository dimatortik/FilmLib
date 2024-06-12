namespace FilmLib.API.Contracts.User;

public record LoginUser(
    string Email,
    string Password
);