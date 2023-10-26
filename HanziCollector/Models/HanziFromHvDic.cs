namespace HanziCollector.Models;

public class HanziFromHvDic
{
    public string Id { get; set; }
    public string Pinyin { get; set; }
    public string HanViet { get; set; }
    public string Cantonese { get; set; }
    public string MeaningInVietnamese { get; set; }
    public bool Processed { get; set; }
}
