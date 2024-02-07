using FluentAssertions;

using Moshaveran.Infrastructure.Mapping;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Infrastructure))]
[Trait("Category", nameof(IMapper))]
public class MapperTests
{
    [Fact]
    public void BasicTest1()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new Person("Ali", 20);

        // Act
        var dst = mapper.Map<Person>(src);

        // Assert
        _ = dst.Should().NotBeNull();
        _ = dst.Name.Should().Match(src.Name);
        _ = dst.Age.Should().Be(src.Age);
    }

    [Fact]
    public void BasicTest2()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new { Name = "Ali", Age = 20 };

        // Act

        // Assert
        _ = Assert.Throws<NotSupportedException>(dst);

        Person dst() => mapper.Map<Person>(src);
    }

    [Fact]
    public void BasicTest3()
    {
        // Assign
        var mapper = IMapper.New();
        var src = new Person("Ali", 20);

        // Act
        var dst = mapper.Map(src, x => new Person(x.Name, x.Age));

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest4()
    {
        // Assign
        var mapper = IMapper.New()
            .ConfigureMapFor<(string Name, int Age), Person>(x => new Person(x.Name, x.Age));

        var src = (Name: "Ali", Age: 20);

        // Act
        var dst = mapper.Map<Person>(src);

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest5()
    {
        // Assign
        var mapper = IMapper.New()
            .ConfigureMapFor<Person, Person>(x => new Person(x.Name, x.Age));
        var src = new Person("Ali", 20);

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

    public Person(string? name, int age)
    {
        this.Name = name;
        this.Age = age;
    }

    public int Age { get; set; }
    public string? Name { get; set; }
}