using AutoMapper;
using IsolatedWorkerAutobot.ValuedObjects;
using Repositories.Models.Vocabulary;

namespace IsolatedWorkerAutobot.Mappers;

public class VocabularyProfile : Profile
{
    public VocabularyProfile()
    {
        CreateMap<CreateVocabularyRequest, Vocabulary>();
    }
}
