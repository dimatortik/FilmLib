namespace FilmLib.Domain.Exceptions;

public record Error(string Message);

public static class DomainException
{
    public static Error EmptyOrOutOfRange(string name) =>
        new( $"{name ?? "Value"} is empty or longer that can be!");

    public static Error EmptyFilmList() => new("List of films is empty");

}