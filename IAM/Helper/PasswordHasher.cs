namespace IAM.Helper;

public static class PasswordHasher
{
  public static string Hash(string password)
  {
    var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
    return hashedPassword;
  }

  public static bool Verify(string password, string hashedPassword)
  {
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}