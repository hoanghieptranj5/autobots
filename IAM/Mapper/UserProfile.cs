using AutoMapper;
using CosmosRepository.Entities.Users;
using IAM.Helper;
using IAM.ValuedObjects;

namespace IAM.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserExport, User>();

        CreateMap<User, UserExport>();

        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opts =>
                opts.MapFrom(src => PasswordHasher.Hash(src.Password)));
    }
}
