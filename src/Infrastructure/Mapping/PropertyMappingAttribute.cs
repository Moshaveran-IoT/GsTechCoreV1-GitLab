namespace Moshaveran.Infrastructure.Mapping;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class PropertyMappingAttribute : Attribute
{
    public PropertyMappingAttribute(string sourceClassName, string sourcePropertyName)
        => this.MapFrom = (sourceClassName, sourcePropertyName);

    public PropertyMappingAttribute(string sourcePropertyName)
        => this.MapFrom = (null, sourcePropertyName);

    public (string? SourceClassName, string SourcePropertyName) MapFrom { get; set; }
}
