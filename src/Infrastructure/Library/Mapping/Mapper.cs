using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Moshaveran.Library.Helpers;

namespace Moshaveran.Library.Mapping;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Mapper : IMapper
{
    private static readonly HashSet<CustomMapper> _customMappers = [];

    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> customMapper)
    {
        _ = _customMappers.Add(CustomMapper.New(customMapper));
        return this;
    }

    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TDestination> customMapper)
    {
        _ = _customMappers.Add(CustomMapper.New<TSource, TDestination>(customMapper));
        return this;
    }

    [return: NotNull]
    public TDestination Map<TDestination>(in object source)
    {
        var destinationType = typeof(TDestination);
        var customMapper = FindCustomMapper(source.GetType(), destinationType, 1);
        if (customMapper != null)
        {
            return (TDestination)customMapper.Map.DynamicInvoke(source)!;
        }
        var ctor = destinationType.GetConstructor([]);
        if (ctor != null)
        {
            var destination = (TDestination)ctor.Invoke([]);
            return Copy(source, destination)!;
        }
        throw new NotSupportedException();
    }

    [return: NotNullIfNotNull(nameof(destination))]
    public TDestination? Map<TDestination>(object source, in TDestination destination)
    {
        var customMapper = FindCustomMapper(source.GetType(), typeof(TDestination), 2);
        return customMapper != null
            ? (TDestination?)customMapper.Map.DynamicInvoke(source, destination)
            : Copy(source, destination);
    }

    public TDestination Map<TDestination>(in object source, Func<TDestination> getDestination)
        => Copy(source, getDestination());

    private static TDestination Copy<TDestination>(object source, TDestination destination)
    {
        var destProps = typeof(TDestination).GetProperties().Compact();
        foreach (var prop in destProps)
        {
            Copy(source, destination, prop);
        }
        return destination;
    }

    private static void Copy<TDestination>(object source, TDestination destination, PropertyInfo dstProp)
    {
        var mapping = dstProp.GetCustomAttribute<PropertyMappingAttribute>()?.MapFrom;
        var name = mapping is { } info && (info.SourceClassName is null || info.SourceClassName == typeof(TDestination).Name)
                ? info.SourcePropertyName ?? dstProp.Name
                : dstProp.Name;
        if (source!.GetType().GetProperty(name) is not { } srcProp)
        {
            return;
        }
        try
        {
            var match = srcProp.GetValue(source) == dstProp.GetValue(destination);
            if (!match)
            {
                try
                {
                    dstProp.SetValue(destination, srcProp.GetValue(source));
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    private static CustomMapper? FindCustomMapper(Type sourceType, Type destinationType, int paramsCount)
        => _customMappers.FirstOrDefault(x => x == (sourceType, destinationType, paramsCount));
}