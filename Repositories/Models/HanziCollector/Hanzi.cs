namespace Repositories.Models.HanziCollector;

public class Hanzi
{
    /// <summary>
    /// Actually this is the character itself
    /// </summary>
    public string Id { get; set; }
    public string? HanViet { get; set; }
    public string? Pinyin { get; set; }
    public string? Cantonese { get; set; }
    public int? Stroke { get; set; }
}