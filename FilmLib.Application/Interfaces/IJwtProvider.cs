using System.Security.Claims;
using FilmLib.Domain.Models;

namespace FilmLib.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
    public ClaimsIdentity? GetClaimsFromToken(string token);
}