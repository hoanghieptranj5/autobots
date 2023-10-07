using AutoMapper;
using IAM.Helper;
using IAM.ValuedObjects;
using Repositories.Models.Users;

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