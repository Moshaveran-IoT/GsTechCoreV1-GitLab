namespace Moshaveran.Infrastructure.Interfaces;

public interface IConvertible<out T>
{
    T Convert();
}

public interface IConverter<in TSource, out TDestination>
{
    static abstract TDestination Convert(TSource source);
}