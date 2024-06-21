using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.User;

public class ChangeUsername
{
    [FromForm(Name = "newUsername")]
    public string? NewUsername { get; set; }
}