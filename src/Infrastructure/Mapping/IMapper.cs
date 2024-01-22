using Moshaveran.Infrastructure.Interfaces;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Moshaveran.Infrastructure.Mapping;

public interface IMappable<in TSource, out TDestination>
{
    TDestination? Map(TSource? self);
}

public interface IMapper
{
    static IMapper New()
        => new Mapper();

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> mapper);

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination, TDestination> mapper);

    TDestination Map<TDestination>(in object source) where TDestination : class, new();

    [return: NotNullIfNotNull(nameof(source))]
    TDestination? Map<TSource, TDestination>(in TSource source, in TDestination destination) where TDestination : class;

    [return: MaybeNull]
    TDestination Map<TDestination>(in object source, in TDestination destination) where TDestination : class;

    TDestination Map<TSelf, TDestination, TMapper>(TSelf source) where TMapper : IMappable<TSelf, TDestination>, new() where TDestination : class, new();

    TDestination? Map<TDestination>(in IConvertible<TDestination> source) where TDestination : class;

    TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination? MapExcept<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except) where TDestination : class, new();
    TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;

    TDestination? MapOnly<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;
}

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Mapper : IMapper
{
    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> mapper) => throw new NotImplementedException();

    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination, TDestination> mapper) => throw new NotImplementedException();

    [return: MaybeNull]
    public TDestination Map<TDestination>(in object source, in TDestination destination)
        where TDestination : class
    {
        if (source == null)
        {
            return null;
        }

        ArgumentNullException.ThrowIfNull(destination);
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            Copy(source, result, prop);
        }

        return result;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public TDestination? Map<TDestination>(in IConvertible<TDestination> source)
        where TDestination : class
        => source?.Convert();

    public TDestination Map<TDestination>(in object source) where TDestination : class, new()
        => this.Map(source, new TDestination())!;

    public TDestination Map<TSelf, TDestination, TMapper>(TSelf source)
        where TMapper : IMappable<TSelf, TDestination>, new()
        where TDestination : class, new()
    {
        var mapper = new TMapper();
        var converter = new ConvertMapperToConverter<TSelf, TDestination, TMapper>(source, mapper);
        return this.Map<TDestination>(converter!);
    }

    public TDestination? Map<TSource, TDestination>(in TSource? source, in TDestination destination)
        where TDestination : class => this.MapExcept<TDestination>(source, destination, default!);

    public TDestination? MapExcept<TDestination>(in object? source, in TDestination destination, in Func<TDestination, object>? except)
        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        ArgumentNullException.ThrowIfNull(destination);

        var exceptProps = (except?.Invoke(destination).GetType().GetProperties().Select(x => x.Name) ?? Enumerable.Empty<object>()).ToArray();
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            if (!exceptProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

    public TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except)
        where TDestination : class, new()
    {
        ArgumentNullException.ThrowIfNull(source);

        var destination = new TDestination();

        var exceptProps = except(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            if (!exceptProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

    public TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new() => throw new NotImplementedException();

    public TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class => throw new NotImplementedException();
    public TDestination? MapOnly<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> onlyProps)
                        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(onlyProps);

        var justProps = onlyProps(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
        var dstProps = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in dstProps)
        {
            if (justProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

    public TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class => throw new NotImplementedException();

    internal static void Copy<TDestination>(object source, TDestination destination, PropertyInfo dstProp)
            where TDestination : class
    {
        var mapping = dstProp.GetCustomAttribute<PropertyMappingAttribute>()?.MapFrom;
        var name = (mapping is { } info) && (info.SourceClassName is null || info.SourceClassName == typeof(TDestination).Name)
                ? info.SourcePropertyName ?? dstProp.Name
                : dstProp.Name;
        if (source!.GetType().GetProperty(name) is { } srcProp)
        {
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
    }
}

internal sealed class ConvertMapperToConverter<TSelf, TDestination, TMapper>(TSelf self, [DisallowNull] TMapper mapper) : IConvertible<TDestination?>
    where TMapper : IMappable<TSelf, TDestination>
{
    public TDestination? Convert()
        => mapper.Map(self);
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class PropertyMappingAttribute : Attribute
{
    public PropertyMappingAttribute(string sourceClassName, string sourcePropertyName)
        => this.MapFrom = (sourceClassName, sourcePropertyName);

    public PropertyMappingAttribute(string sourcePropertyName)
        => this.MapFrom = (null, sourcePropertyName);

    public (string? SourceClassName, string SourcePropertyName) MapFrom { get; set; }
}