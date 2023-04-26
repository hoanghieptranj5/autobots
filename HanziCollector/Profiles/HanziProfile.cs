using AutoMapper;
using HanziCollector.Models;
using Repositories.Models.HanziCollector;

namespace HanziCollector.Profiles;

public class HanziProfile : Profile
{
  public HanziProfile()
  {
    CreateMap<HanziFromHvDic, Hanzi>();
  }
}