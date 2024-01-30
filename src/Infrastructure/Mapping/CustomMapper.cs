using System.Numerics;

namespace Moshaveran.Infrastructure.Mapping;

public sealed class CustomMapper : IEquatable<CustomMapper>
    , IEquatable<(Type SourceType, Type DestinationType)>
    , IEqualityOperators<CustomMapper, CustomMapper, bool>
    , IEqualityOperators<CustomMapper, (Type SourceType, Type DestinationType, int ParamsCount), bool>
{
    private readonly Type _destinationType;
    private readonly int _paramsCount;
    private readonly Type _sourceType;

    private CustomMapper(in Type sourceType, in Type DestinationType, in Delegate map)
    {
        this._destinationType = DestinationType;
        this._sourceType = sourceType;
        this.Map = map;
        _paramsCount = map.Method.GetParameters().Count();
    }

    public Delegate Map { get; }

    public static bool Equals(CustomMapper? x, CustomMapper? y) => (x, y) switch
    {
        (null, null) => true,
        (null, _) or (_, null) => false,
        _ => x.GetHashCode() == y.GetHashCode()
    };

    public static bool Equals(CustomMapper? x, (Type SourceType, Type DestinationType, int ParamsCount) y) => (x, y) switch
    {
        (null, _) => false,
        (_, (not null, not null, > -1)) => x.GetHashCode() == GetHashCode(y.SourceType, y.DestinationType, y.ParamsCount),
        _ => false
    };

    public static CustomMapper New<TSource, TDestination>(Func<TSource, TDestination> getDestination)
        => new(typeof(TSource), typeof(TDestination), getDestination);

    public static CustomMapper New<TSource, TDestination>(Func<TDestination> getDestination)
        => new(typeof(TSource), typeof(TDestination), getDestination);

    public static bool operator !=(CustomMapper? left, CustomMapper? right)
        => !Equals(left, right);

    public static bool operator !=(CustomMapper? left, (Type SourceType, Type DestinationType, int ParamsCount) right)
        => !Equals(left, right);

    public static bool operator ==(CustomMapper? left, CustomMapper? right)
        => Equals(left, right);

    public static bool operator ==(CustomMapper? left, (Type SourceType, Type DestinationType, int ParamsCount) right)
        => Equals(left, right);

    public void Deconstruct(out Type source, out Type destination)
        => (source, destination) = (this._sourceType, this._destinationType);

    public void Deconstruct(out Type source, out Type destination, out Delegate map)
        => (source, destination, map) = (this._sourceType, this._destinationType, this.Map);

    public bool Equals(CustomMapper? other)
        => Equals(this, other);

    public override bool Equals(object? obj)
        => Equals(this, obj as CustomMapper);

    public bool Equals((Type SourceType, Type DestinationType) other)
        => Equals(this, other);

    public override int GetHashCode()
        => GetHashCode(_sourceType, this._destinationType, _paramsCount);

    private static int GetHashCode(Type sourceType, Type destinationType, int paramsCount)
        => HashCode.Combine(sourceType, destinationType, paramsCount);
}