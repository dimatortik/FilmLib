namespace FilmLib.Domain.Exceptions;

public record Error(string Message);

public static class DomainException
{
    public static class ValueObjects
    {
        public static Error RatingEmptyOrOutOfRage() =>
            new( $"Rating must be from 0 to 10!");

        public static Error OutOfRange(string name) => new($"{name} is out of range");
    }
    public static Error EmptyOrOutOfRange(string name) =>
        new( $"{name ?? "Value"} is empty or longer that can be!");

    public static Error EmptyFilmList() => new("List of films is empty");

    public static Error OutOfRange(string name) => new($"{name} is out of range");

}
