namespace IsolatedWorkerAutobot.CustomAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowAllAttribute : Attribute
{
}