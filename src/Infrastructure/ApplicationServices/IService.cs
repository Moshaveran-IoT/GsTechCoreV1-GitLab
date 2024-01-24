namespace Moshaveran.Infrastructure.ApplicationServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ServiceAttribute : Attribute
{
}

public interface IBusinessService
{
}