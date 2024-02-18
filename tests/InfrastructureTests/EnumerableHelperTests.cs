using static Moshaveran.Library.Helpers.EnumerableHelper;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Helpers))]
[Trait("Category", nameof(EnumerableHelper))]
public sealed class EnumerableHelperTests
{
    [Fact]
    public void Build_ReturnsEmptyCollectionWhenValuesIsEmptyInt()
    {
        // Arrange
        var values = Enumerable.Empty<int>();

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<int>());
    }

    [Fact]
    public void Build_ReturnsImmutableArrayOfValues()
    {
        // Arrange
        IEnumerable<int> values = new int[] { 1, 2, 3 };

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Build_ReturnsNullWhenValuesIsNull()
    {
        // Arrange
        IEnumerable<int>? values = null;

        // Act
        var result = values.Build();

        // Assert
        _ = result.Should().BeNull();
    }

    [Fact]
    public void Combine_CombinesMixedCollectionsWithEmptyAndNonEmpty()
    {
        // Arrange
        var items = new List<IEnumerable<int>> { Enumerable.Empty<int>(), new[] { 1, 2, 3 }, Enumerable.Empty<int>(), new[] { 4, 5, 6 } };

        // Act
        var result = items.Combine();

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6 });
    }

    [Fact]
    public void Combine_CombinesNonEmptyCollections()
    {
        // Arrange
        var items = new List<IEnumerable<int>> { new[] { 1, 2, 3 }, new[] { 4, 5, 6 } };

        // Act
        var result = items.Combine();

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6 });
    }

    [Fact]
    public void Combine_ReturnsEmptyCollectionWhenAllSubCollectionsAreEmpty()
    {
        // Arrange
        var items = new List<IEnumerable<int>> { Enumerable.Empty<int>(), Enumerable.Empty<int>() };

        // Act
        var result = items.Combine();

        // Assert
        _ = result.Should().BeEmpty();
    }

    [Fact]
    public void Combine_ThrowsArgumentNullExceptionWhenItemsIsNull()
    {
        // Arrange
        IEnumerable<IEnumerable<int>>? items = null;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => items!.Combine());
    }

    [Fact]
    public void Compact_FiltersOutNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", null!, "two", null!];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two");
    }

    [Fact]
    public void Compact_HandlesMixedCollectionWithNullAndNonNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", null!, "two", null!, "three"];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two", "three");
    }

    [Fact]
    public void Compact_ReturnsEmptyCollectionWhenItemsIsEmptyInt()
    {
        // Arrange
        var items = Enumerable.Empty<IEnumerable<int>>();

        // Act
        var result = items.Combine();

        // Assert
        _ = result.Should().BeEmpty();
    }

    [Fact]
    public void Compact_ReturnsEmptyCollectionWhenItemsIsEmptyString()
    {
        // Arrange
        var items = Enumerable.Empty<string>();

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<string>());
    }

    [Fact]
    public void Compact_ReturnsEmptyCollectionWhenItemsIsNullString()
    {
        // Arrange
        IEnumerable<string>? items = null;

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo(Enumerable.Empty<string>());
    }

    [Fact]
    public void Compact_ReturnsOriginalCollectionIfNoNullValues()
    {
        // Arrange
        IEnumerable<string> items = ["one", "two", "three"];

        // Act
        var result = items.Compact();

        // Assert
        _ = result.Should().BeEquivalentTo("one", "two", "three");
    }

    [Fact]
    public async Task Enumerate_DoesNothingWhenValuesIsEmpty()
    {
        // Arrange
        var values = Enumerable.Empty<int>();
        var token = new CancellationToken();

        // Act
        await values.Enumerate(Enumerate_TestAction, token);

        // Assert No exception should be thrown
    }

    [Fact]
    public async Task Enumerate_DoesNothingWhenValuesIsNull()
    {
        // Arrange
        IEnumerable<int>? values = null;
        var token = new CancellationToken();

        // Act
        await values!.Enumerate(Enumerate_TestAction, token);

        // Assert No exception should be thrown
    }

    [Fact]
    public async Task Enumerate_ExecutesActionForEachItem()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };
        var token = new CancellationToken();
        var actionExecuted = false;

        async Task Action(int value, CancellationToken token)
        {
            actionExecuted = true;
            await Task.CompletedTask;
        }

        // Act
        await values.Enumerate(Action, token);

        // Assert
        _ = actionExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task Enumerate_StopsEnumerationWhenTokenIsCancelled()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        async Task Action(int value, CancellationToken token)
        {
            if (value == 2)
            {
                tokenSource.Cancel();
            }
            await Task.CompletedTask;
        }

        // Act
        await values.Enumerate(Action, token);

        // Assert No exception should be thrown
    }

    [Fact]
    public async Task Enumerate_ThrowsArgumentNullExceptionWhenActionIsNull()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };
        Func<int, CancellationToken, Task>? action = null;
        var token = new CancellationToken();

        // Act & Assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() => values.Enumerate(action!, token));
    }

    [Fact]
    public void ToEnumerable_ReturnsEnumerableWithSingleItemWhenItemIsNotNull()
    {
        // Arrange
        var item = "test";

        // Act
        var result = ToEnumerable(item);

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { "test" });
    }

    [Fact]
    public void ToEnumerable_ReturnsEnumerableWithSingleItemWhenItemIsNull()
    {
        // Arrange
        string? item = null;

        // Act
        var result = ToEnumerable(item);

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { (string?)null });
    }

    [Fact]
    public void ToEnumerable_ReturnsEnumerableWithSingleItemWhenItemIsReferenceType()
    {
        // Arrange
        var item = new int();

        // Act
        var result = ToEnumerable(item);

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { new int() });
    }

    [Fact]
    public void ToEnumerable_ReturnsEnumerableWithSingleItemWhenItemIsValueType()
    {
        // Arrange
        var item = 42;

        // Act
        var result = ToEnumerable(item);

        // Assert
        _ = result.Should().BeEquivalentTo(new[] { 42 });
    }

    private static async Task Enumerate_TestAction(int value, CancellationToken token) =>
                        // Simulate some asynchronous operation
                        await Task.Delay(100, token);
}