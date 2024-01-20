using System.Reflection;

namespace Moshaveran.Infrastructure;

public interface IMapper
{
    IList<(Type Source, Type Destination, Delegate Converter)> Maps { get; }

    static IMapper New() =>
        new Mapper();

    T Map<T>(in object o) where T : new();

    T Map<T>(in T o) where T : new();

    T Map<T>(in object o, in T destination);
}

internal sealed class Mapper : IMapper
{
    public IList<(Type Source, Type Destination, Delegate Converter)> Maps { get; } = new List<(Type Source, Type Destination, Delegate Converter)>();

    public T Map<T>(in object o)
        where T : new()
    {
        var result = new T();
        return Map(o, result);
    }

    public T Map<T>(in T o)
        where T : new()
    {
        var result = new T();
        return Map(o, result);
    }

    public T Map<T>(in object o, in T destination)
    {
        CopyAll(o, destination);
        return destination;
    }

    private void CopyAll(object src, object dst)
    {
        var converter = this.Maps.Where(x => x.Source == src.GetType() && x.Destination == dst.GetType()).FirstOrDefault().Converter;
        if(converter!=default)
        {
            dst = converter.DynamicInvoke(src, dst);
            
        }
        var dstProps = dst.GetType().GetProperties();
        foreach (var prop in dstProps)
        {
            CopyByDstPropName(src, dst, prop);
        }
    }

    private static void CopyByDstPropName(object src, object dst, PropertyInfo dstProp)
    {
        if (src.GetType().GetProperty(dstProp.Name) is { } srcProp)
        {
            try
            {
                dstProp.SetValue(dst, srcProp.GetValue(src));
            }
            catch
            {
            }
        }
    }
}

public static class MapperExtensions
{
    public static IMapper MapFor<TSource, TDestinations>(this IMapper mapper, Func<TSource, TDestinations> converter)
    {
        mapper.Maps.Add((typeof(TSource), typeof(TDestinations), converter));
        return mapper;
    }
}