namespace FilmLib.Application.FilmComments.Queries;

public class FilmCommentResponse
{
    public Guid Id { get; set; }
    
    public string Body { get; set; }
    
    public DateTime CreatedAt { get; private set; }

    public string UserName { get; set; }

}