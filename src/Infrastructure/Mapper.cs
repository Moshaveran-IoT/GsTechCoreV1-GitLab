using System.Reflection;

namespace Moshaveran.Infrastructure;

public interface IMapper
{
    T Map<T>(in object o) where T : new();

    T Map<T>(in T o) where T : new();

    T Map<T>(in object o, in T destination);

    static IMapper New() =>
        new Mapper();
}

internal sealed class Mapper : IMapper
{
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
        var dstProps = dst.GetType().GetProperties();
        foreach (var prop in dstProps)
        {
            CopyByDstPropName(src, dst, prop);
        }
    }

    private void CopyByDstPropName(object src, object dst, PropertyInfo dstProp)
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