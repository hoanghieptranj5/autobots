using AutoMapper;
using ElectricCalculator.Models;
using Repositories.Models.ElectricCalculator;

namespace ElectricCalculator.Profiles;

public class ElectricPriceProfile : Profile
{
    public ElectricPriceProfile()
    {
        CreateMap<CreateElectricPriceRequestModel, ElectricPrice>();
    }
}