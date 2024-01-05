namespace EzTech.Data.DtoModels;

public class ImageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string AltText { get; set; } = null!;
}