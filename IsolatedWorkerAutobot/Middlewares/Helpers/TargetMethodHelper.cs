using System.Reflection;

namespace IsolatedWorkerAutobot.Middlewares.Helpers;

internal static class TargetMethodHelper
{
    public static List<T> GetFunctionMethodAttribute<T>(MethodInfo targetMethod) where T : Attribute
    {
        var methodAttributes = targetMethod.GetCustomAttributes<T>();
        var classAttributes = targetMethod.DeclaringType.GetCustomAttributes<T>();
        return methodAttributes.Concat(classAttributes).ToList();
    }

    public static MethodInfo GetTargetFunctionMethod(FunctionContext context)
    {
        var assemblyPath = context.FunctionDefinition.PathToAssembly;
        var assembly = Assembly.LoadFrom(assemblyPath);
        var typeName =
            context.FunctionDefinition.EntryPoint.Substring(0, context.FunctionDefinition.EntryPoint.LastIndexOf('.'));
        var type = assembly.GetType(typeName);
        var methodName =
            context.FunctionDefinition.EntryPoint.Substring(context.FunctionDefinition.EntryPoint.LastIndexOf('.') + 1);
        var method = type.GetMethod(methodName);
        return method;
    }
}
