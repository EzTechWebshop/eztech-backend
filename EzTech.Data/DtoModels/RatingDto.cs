namespace EzTech.Data.DtoModels;

public class RatingDto
{
    public int Id { get; set; }
    public int Rate { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}