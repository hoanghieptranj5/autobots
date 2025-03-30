using AutoMapper;
using CosmosRepository.Entities.ElectricCalculator;
using ElectricCalculator.Models;

namespace ElectricCalculator.Profiles;

public class ElectricPriceProfile : Profile
{
    public ElectricPriceProfile()
    {
        CreateMap<CreateElectricPriceRequestModel, ElectricPrice>();
    }
}
