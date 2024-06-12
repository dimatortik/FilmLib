using FilmLib.Domain.Models;
using FilmLib.Domain.ValueObjects;

namespace FilmLib.Application.Films.Queries.GetById;

public class FilmResponse
{
    public Guid Id { get; set; }
    public string TitleImageLink { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Views { get; set; }
    public double Rating { get; set; }
    public int? Year{ get; set; }
    public string? Country { get;  set;}
    public string? Director { get;  set;}
}