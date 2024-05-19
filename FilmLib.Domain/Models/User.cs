using CSharpFunctionalExtensions;
using FilmLib.Domain.Exceptions;

namespace FilmLib.Domain.Models;

public class User
{ 
    public const int MAX_USERNAME_LENGTH = 25;
    public const int MAX_EMAIL_LENGTH = 30;
    
    
    private User()
    {
    }
    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set;  }

    public string Username { get; private set; }

    public static Result<User> Create(string email, string passwordHash, string username)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Username = username

        };
            
        if (string.IsNullOrWhiteSpace(email) || email.Length > MAX_EMAIL_LENGTH)
        {
            return Result.Failure<User>(DomainException.EmptyOrOutOfRange("Email").Message);
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>(DomainException.EmptyOrOutOfRange("PasswordHash").Message);
        }

        if (string.IsNullOrWhiteSpace(username) || username.Length > MAX_USERNAME_LENGTH )
        {
            return Result.Failure<User>(DomainException.EmptyOrOutOfRange("Username").Message);
        }
        
        return Result.Success(user);
    }
    
    
    public Result ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || newEmail.Length > MAX_EMAIL_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange("Email").Message);
        }

        Email = newEmail;
        return Result.Success();
    }
    public Result ChangeUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername) || newUsername.Length > MAX_USERNAME_LENGTH)
        {
            return Result.Failure(DomainException.EmptyOrOutOfRange("Username").Message);
        }

        Username = newUsername;
        return Result.Success();
    }
}