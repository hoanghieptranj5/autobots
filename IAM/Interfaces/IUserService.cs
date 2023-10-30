using IAM.ValuedObjects;

namespace IAM.Interfaces;

public interface IUserService
{
    Task<UserExport?> CreateSingle(CreateUserRequest request);
    Task<UserExport> UpdateSingle(CreateUserRequest request);
    Task<IEnumerable<UserExport>> GetList();
    Task<UserExport> DeleteSingle(DeleteUserRequest request);
    Task<UserExport> DeleteSingleByUsername(string username);
    Task<UserExport> DeleteSingleByEmail(string username);
    Task<string> Login(LoginRequest request);
    Task<UserExport> Logout(LogoutRequest request);
}
