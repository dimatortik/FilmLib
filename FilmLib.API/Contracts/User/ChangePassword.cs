using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Contracts.User;

public class ChangePassword
{
    [FromForm(Name = "oldPassword")]
    public string? OldPassword { get; set; }
    
    [FromForm(Name = "newPassword")]
    public string? NewPassword { get; set; }
}
