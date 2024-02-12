namespace Moshaveran.Library.Mapping;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class PropertyMappingAttribute : Attribute
{
    public PropertyMappingAttribute(string sourceClassName, string sourcePropertyName)
        => MapFrom = (sourceClassName, sourcePropertyName);

    public PropertyMappingAttribute(string sourcePropertyName)
        => MapFrom = (null, sourcePropertyName);

    public (string? SourceClassName, string SourcePropertyName) MapFrom { get; set; }
}
