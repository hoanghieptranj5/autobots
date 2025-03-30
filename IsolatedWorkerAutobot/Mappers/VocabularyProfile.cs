using AutoMapper;
using CosmosRepository.Entities.Vocabulary;
using IsolatedWorkerAutobot.ValuedObjects;

namespace IsolatedWorkerAutobot.Mappers;

public class VocabularyProfile : Profile
{
    public VocabularyProfile()
    {
        CreateMap<CreateVocabularyRequest, Vocabulary>();
    }
}
