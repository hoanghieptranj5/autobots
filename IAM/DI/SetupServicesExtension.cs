using IAM.Interfaces;
using IAM.Mapper;
using IAM.Services;
using Microsoft.Extensions.DependencyInjection;
using Repositories.UnitOfWork;
using Repositories.UnitOfWork.Abstractions;

namespace IAM.DI;

public static class SetupServicesExtension
{
  public static void SetupIAMDependencies(this IServiceCollection services)
  {
    #region Internal Interfaces

    services.AddScoped<IUnitOfWork, UnitOfWork>();

    #endregion

    #region Public Interfaces

    services.AddAutoMapper(typeof(UserProfile).Assembly);

    services.AddScoped<IUserService, UserService>();

    #endregion
  }
}