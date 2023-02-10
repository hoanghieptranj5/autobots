namespace Repositories.Models;

public class Car
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public long Price { get; set; }
    public string? Description { get; set; }
    public int Year { get; set; }
    public DateTime CollectedDate { get; set; }
    public string? CollectedFrom { get; set; }
}