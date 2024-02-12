using Moshaveran.Library.Helpers;
using static Moshaveran.Library.Helpers.ObjectHelper;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Helpers))]
[Trait("Category", nameof(ObjectHelper))]
public sealed class ObjectHelperTests
{
    [Trait("Category", nameof(Pretty))]
    [Fact]
    public void FormatsObjectWithMultipleProperties()
    {
        // Arrange
        var input = new TestObject { Name = "John Doe", Age = 30 };

        // Act
        var result = Pretty(input);

        // Assert
        _ = result.Should().Be("Age : 30, Name : John Doe");
    }

    [Trait("Category", nameof(Pretty))]
    [Fact]
    public void ReturnsEmptyStringWhenInputHasNoProperties()
    {
        // Arrange
        var input = new object();

        // Act
        var result = Pretty(input);

        // Assert
        _ = result.Should().BeEmpty();
    }

    [Trait("Category", nameof(Pretty))]
    [Fact]
    public void ReturnsEmptyStringWhenInputIsNull()
    {
        // Arrange
        object? input = null;

        // Act
        var result = Pretty(input);

        // Assert
        _ = result.Should().BeEmpty();
    }

    [Trait("Category", nameof(Pretty))]
    [Fact]
    public void TrimsTrailingSeparator()
    {
        // Arrange
        var input = new TestObject { Name = "John Doe", Age = 30 };

        // Act
        var result = Pretty(input);

        // Assert
        _ = result.Should().NotEndWith(", ");
        _ = result.Should().NotContain(" , ");
    }

    [Trait("Category", nameof(Pretty))]
    [Fact]
    public void UsesCustomSeparatorWhenSpecified()
    {
        // Arrange
        var input = new TestObject { Name = "John Doe", Age = 30 };
        const char customSeparator = ';';

        // Act
        var result = Pretty(input, customSeparator);

        // Assert
        _ = result.Should().Be("Age : 30; Name : John Doe");
    }

    private class TestObject
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}