namespace IsolatedWorkerAutobot.ValuedObjects;

public record CreateVocabularyRequest
{
    public string Word { get; set; }
    public string Category { get; set; }
    public string Example { get; set; }
    public string Meaning { get; set; }
    public string Cantonese { get; set; }
    public string Pinyin { get; set; }
}
