namespace FilmLib.API.Contracts.User;

public record ChangePassword(
    string Password,
    string NewPassword);