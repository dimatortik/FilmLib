using System.Security.Claims;
using CSharpFunctionalExtensions;
using FilmLib.Application.Interfaces;
using FilmLib.Domain.Enums;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Auth;

public class AuthService(
    IPasswordHasher passwordHasher, 
    AppDbContext context,
    IJwtProvider jwtProvider)
{

    public async Task<Result<string>> Register(string email, string password, string username)
    {
        var passwordHash = passwordHasher.GenerateHash(password);
    
        var userExists = await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
        if (userExists)
            return Result.Failure<string>("User with the same email already exists.");
    
        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == nameof(Role.User));
        
        var user = User.Create(email, passwordHash, username);
        if (user.IsFailure)
            return Result.Failure<string>(user.Error);
    
        user.Value.Roles.Add(userRole);
    
        await context.Users.AddAsync(user.Value);
    
        await context.SaveChangesAsync();
    
        var loginResult = await Login(email, password);
        if (loginResult.IsFailure)
            return Result.Failure<string>(loginResult.Error);
    
        return Result.Success(loginResult.Value);
    }
    
    public async Task<Result<string>> Login(string email, string password)
    {
        
        var user = await context.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return Result.Failure<string>("User not found.");

        if (!passwordHasher.VerifyHash(password, user.PasswordHash))
            return Result.Failure<string>("Invalid password.");

        var token = jwtProvider.GenerateJwtToken(user);
        
        return Result.Success(token);
    }
    
    public async Task<Result> ChangePassword(Guid id, string oldPassword, string newPassword)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return Result.Failure("User not found.");
        
        if (!passwordHasher.VerifyHash(oldPassword, user.PasswordHash))
            return Result.Failure("Invalid password.");
        
        var newPasswordHash = passwordHasher.GenerateHash(newPassword);
        user.ChangePassword(newPasswordHash);
        
        await context.SaveChangesAsync();
        
        return Result.Success();
    }
    
    public async Task<Result> ChangeUsername(Guid id, string newUsername)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return Result.Failure("User not found.");
        
        user.ChangeUsername(newUsername);
        
        await context.SaveChangesAsync();
        
        return Result.Success();
    }
    
    public Result<Guid> GetUserId(string jwt)
    {
        var claims = jwtProvider.GetClaimsFromToken(jwt)?.Claims;
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        if (userId == null)
            return Result.Failure<Guid>("User not found.");
        
        return Result.Success(Guid.Parse(userId));
    }
    
    
}




