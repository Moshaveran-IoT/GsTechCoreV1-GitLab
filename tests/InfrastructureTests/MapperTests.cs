using Moshaveran.Library.Mapping;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Mapping))]
public class MapperTests(IMapper mapper)
{
    private readonly IMapper _mapper = mapper;

    [Fact]
    public void BasicTest1()
    {
        // Assign
        var src = new Person("Ali", 20);

        // Act
        var dst = this._mapper.Map<Person>(src);

        // Assert
        _ = dst.Should().NotBeNull();
        _ = dst.Name.Should().Match(src.Name);
        _ = dst.Age.Should().Be(src.Age);
    }

    [Fact]
    public void BasicTest2()
    {
        // Assign
        var src = new Animal("Cat");
        Animal dst() => this._mapper.Map<Animal>(src);

        // Act

        // Assert
        _ = Assert.Throws<NotSupportedException>(dst);
    }

    [Fact]
    public void BasicTest3()
    {
        // Assign
        var src = new Person("Ali", 20);

        // Act
        var dst = this._mapper.Map(src, x => new Person(x.Name, x.Age));

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest4()
    {
        // Assign
        _ = this._mapper.ConfigureMapFor<(string Name, int Age), Person>(x => new Person(x.Name, x.Age));

        var src = (Name: "Ali", Age: 20);

        // Act
        var dst = this._mapper.Map<Person>(src);

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }

    [Fact]
    public void BasicTest5()
    {
        // Assign
        _ = this._mapper.ConfigureMapFor<Person, Person>(x => new Person(x.Name, x.Age));
        var src = new Person("Ali", 20);

        // Act
        var dst = this._mapper.Map<Person>(src);

        // Assert
        Assert.Equal(src.Name, dst.Name);
        Assert.Equal(src.Age, dst.Age);
    }
}

internal sealed class Person()
{
    public Person(string name, int age)
        : this()
    {
        this.Name = name;
        this.Age = age;
    }

    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
}

file sealed record Animal(string Name);