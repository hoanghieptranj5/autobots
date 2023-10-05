namespace IsolatedWorkerAutobot.CustomAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizedAttribute : Attribute
{
  public string[] UserRoles { get; set; } = Array.Empty<string>();
}