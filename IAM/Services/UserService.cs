using AutoMapper;
using CosmosRepository.Abstractions;
using CosmosRepository.Entities.Users;
using IAM.Helper;
using IAM.Interfaces;
using IAM.ValuedObjects;


namespace IAM.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

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
        await _unitOfWork.SaveChangesAsync();

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
        if (user == null || !enumerable.Any()) throw new Exception("User not found");

        await _unitOfWork.Users.Delete(enumerable.FirstOrDefault()!.Id);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserExport>(user);
    }

    public Task<UserExport> DeleteSingleByEmail(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Login(LoginRequest request)
    {
        var users = await _unitOfWork.Users.Find(u => u.Username.Equals(request.Username));
        var userList = users.ToList();
        if (!userList.Any()) throw new Exception("Username is not found.");

        var user = userList.FirstOrDefault(u => PasswordHasher.Verify(request.Password, u.PasswordHash));
        if (user == null) throw new Exception("Invalid Password.");
        var token = JwtHelper.GenerateToken(user.Id, user.Email);

        return token;
    }

    public Task<UserExport> Logout(LogoutRequest request)
    {
        throw new NotImplementedException();
    }
}
