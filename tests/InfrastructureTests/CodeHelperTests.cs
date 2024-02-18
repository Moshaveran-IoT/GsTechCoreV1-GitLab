using Moshaveran.Library;

using static Moshaveran.Library.CodeHelper;

namespace InfrastructureTests;

public sealed class CodeHelperTests
{
    [Fact]
    public void New_ShouldCreateNewInstanceOfTypePerson()
    {
        // Arrange No setup needed as the method under test creates a new instance.

        // Act
        var instance = New<Person>();

        // Assert
        Assert.NotNull(instance);
        _ = Assert.IsType<Person>(instance);
    }

    [Fact]
    public void WithAction_ShouldInvokeActionOnNonNullObject()
    {
        // Arrange
        const string originalObject = "Original Value";
        var actionInvoked = false;
        var action = new Action<string?>(_ => actionInvoked = true);

        // Act
        var result = originalObject.With(action);

        // Assert
        Assert.True(actionInvoked);
        Assert.Equal("Original Value", result);
    }

    [Fact]
    public void WithAction_ShouldReturnOriginalObjectOnNullAction()
    {
        // Arrange
        const string originalObject = "Original Value";
        Action<string?>? action = null;

        // Act
        var result = originalObject.With(action!);

        // Assert
        Assert.Equal("Original Value", result);
    }

    [Fact]
    public void WithAction_ShouldReturnOriginalObjectOnNullObject()
    {
        // Arrange
        const string? originalObject = null;
        var action = new Action<string?>(_ => { });

        // Act
        var result = originalObject.With(action);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void WithFunc_ShouldApplyActionOnNonNullObject()
    {
        // Arrange
        const string originalObject = "Original Value";
        var action = new Func<string?, string?>(s => s + " Modified");

        // Act
        var result = originalObject.With(action);

        // Assert
        Assert.Equal("Original Value Modified", result);
    }

    [Fact]
    public void WithFunc_ShouldReturnOriginalObjectOnNullAction()
    {
        // Arrange
        const string originalObject = "Original Value";
        Func<string?, string?>? action = null;

        // Act
        var result = originalObject.With(action!);

        // Assert
        Assert.Equal("Original Value", result);
    }

    [Fact]
    public void WithFunc_ShouldReturnOriginalObjectOnNullObject()
    {
        // Arrange
        const string? originalObject = null;
        var action = new Func<string?, string?>(s => s + " Modified");

        // Act
        var result = originalObject.With(action);

        // Assert
        Assert.Equal(" Modified", result);
    }
}

file record Person();