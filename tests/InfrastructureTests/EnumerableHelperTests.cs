using Moshaveran.Library.Helpers;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Helpers))]
[Trait("Category", nameof(EnumerableHelper))]
public sealed class EnumerableHelperTests
{
    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Compact))]
    public void FiltersOutNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", null!, "two", null!];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two");
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Compact))]
    public void HandlesMixedCollectionWithNullAndNonNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", null!, "two", null!, "three"];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two", "three");
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Compact))]
    public void ReturnsEmptyCollectionWhenItemsIsEmpty()
    {
        // Arrange
        var items = Enumerable.Empty<string>();

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<string>());
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Compact))]
    public void ReturnsEmptyCollectionWhenItemsIsNull()
    {
        // Arrange
        IEnumerable<string>? items = null;

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<string>());
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Build))]
    public void ReturnsEmptyCollectionWhenValuesIsEmpty()
    {
        // Arrange
        var values = Enumerable.Empty<int>();

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<int>());
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Build))]
    public void ReturnsImmutableArrayOfValues()
    {
        // Arrange
        IEnumerable<int> values = new int[] { 1, 2, 3 };

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Build))]
    public void ReturnsNullWhenValuesIsNull()
    {
        // Arrange
        IEnumerable<int>? values = null;

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", nameof(EnumerableHelper.Compact))]
    public void ReturnsOriginalCollectionIfNoNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", "two", "three"];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two", "three");
    }
}