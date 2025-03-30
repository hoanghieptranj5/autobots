namespace CosmosRepository.Entities.HanziCollector;

public class Hanzi : BaseEntity
{
    public string? HanViet { get; set; }
    public string? Pinyin { get; set; }
    public string? Cantonese { get; set; }
    public int? Stroke { get; set; }
    public string? MeaningInVietnamese { get; set; }

    /// <summary>
    /// Indexed field, used for quickly randomly access a Hanzi Character
    /// </summary>
    public int InsertedOrder { get; set; }
}
