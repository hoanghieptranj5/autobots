using AutoMapper;
using IAM.Data.Abstraction;
using IAM.Models;
using Repositories.Models.Users;
using Repositories.UnitOfWork.Abstractions;

namespace IAM.Data;

public class UserService : IUserService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;

  public UserService(IUnitOfWork unitOfWork, IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }

  public async Task<UserExport?> CreateSingle(CreateUserRequest request)
  {
    var savingUser = _mapper.Map<User>(request);
    savingUser.CreatedAt = DateTime.Now;
    savingUser.UpdatedAt = DateTime.Now;
    var result = await _unitOfWork.Users.Add(savingUser);
    await _unitOfWork.CompleteAsync();

    return result ? _mapper.Map<UserExport>(savingUser) : null;
  }

  public Task<UserExport> UpdateSingle(CreateUserRequest request)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<UserExport>> GetList()
  {
    var users = await _unitOfWork.Users.All();
    return users.Select(u => _mapper.Map<UserExport>(u));
  }

  public Task<UserExport> DeleteSingle(DeleteUserRequest request)
  {
    throw new NotImplementedException();
  }

  public async Task<UserExport> DeleteSingleByUsername(string username)
  {
    var user = await _unitOfWork.Users.Find(u => u.Username.Equals(username));
    var enumerable = user.ToList();
    if (user == null || !enumerable.Any())
    {
      throw new Exception("User not found");
    }

    await _unitOfWork.Users.Delete(enumerable.FirstOrDefault()!.Id);
    await _unitOfWork.CompleteAsync();
    return _mapper.Map<UserExport>(user);
  }

  public Task<UserExport> DeleteSingleByEmail(string username)
  {
    throw new NotImplementedException();
  }
}