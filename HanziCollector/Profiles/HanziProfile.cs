using AutoMapper;
using CosmosRepository.Entities.HanziCollector;
using HanziCollector.Models;

namespace HanziCollector.Profiles;

public class HanziProfile : Profile
{
    public HanziProfile()
    {
        CreateMap<HanziFromHvDic, Hanzi>();
    }
}
