using Moshaveran.Infrastructure;

namespace InfrastructureTests;

public class MapperTests
{
    [Fact]
    public void BasicTest1()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new Person("Ali", 20);

        // Act
        var dst = mapper.Map(src);

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest2()
    {
        // Assign
        var mapper = IMapper.New();

        // Act
        var src = new { Name = "Ali", Age = 20 };
        var dst = mapper.Map(src, new Person());

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest3()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new Person("Ali", 20);

        // Act
        var dst = mapper.Map(src, new Person());

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest4()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new { Name = "Ali", Age = 20 };

        // Act
        var dst = mapper.Map<Person>(src);

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }
}

internal sealed class Person
{
    public Person()
    {
    }

    public Person(string? name, int age) =>
        (this.Name, this.Age) = (name, age);

    public int Age { get; set; }
    public string? Name { get; set; }

    public override bool Equals(object? obj) =>
        Equals(obj as Person);

    public override int GetHashCode() =>
        HashCode.Combine(this.Name?.GetHashCode() ?? 0, this.Age.GetHashCode());
}

public static class SimpleEquitable
{
    public static bool Equals(this object? me, object? other)
    {
        return me == null ? other == null : other == null ? false : me.GetHashCode() == other.GetHashCode();
    }
}