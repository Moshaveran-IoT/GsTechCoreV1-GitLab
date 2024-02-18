using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Validations;

namespace InfrastructureTests;

public sealed class CheckTests
{
    [Fact]
    public void ArgumentNotNull_ShouldReturnArgumentOnNonNullArgument()
    {
        // Arrange
        var nonNullArg = "Non-null value";

        // Act
        var result = nonNullArg.ArgumentNotNull();

        // Assert
        Assert.Equal("Non-null value", result);
    }

    [Fact]
    public void ArgumentNotNull_ShouldThrowOnNullArgument()
    {
        // Arrange
        string? nullArg = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => nullArg.ArgumentNotNull());
        Assert.Equal("nullArg", ex.ParamName);
    }

    [Fact]
    public void MustBe_Generic_ShouldNotThrowExceptionOnTrueCondition()
    {
        // Arrange
        var condition = true;

        // Act & Assert
        Check.MustBe<GsTechException>(condition);
        // No exception should be thrown, so no assertion is needed here.
    }

    [Fact]
    public void MustBe_Generic_ShouldThrowSpecificExceptionOnFalseCondition()
    {
        // Arrange
        var condition = false;

        // Act & Assert
        var ex = Assert.Throws<GsTechException>(() => Check.MustBe<GsTechException>(condition));
        // Additional assertions can be made here if needed, such as checking the exception message.
    }

    [Fact]
    public void MustBe_ShouldNotThrowExceptionOnTrueCondition()
    {
        // Arrange
        var condition = true;
        var exception = new ArgumentException("Invalid argument");

        // Act & Assert
        Check.MustBe(condition, () => exception);
        // No exception should be thrown, so no assertion is needed here.
    }

    [Fact]
    public void MustBe_ShouldThrowSpecificExceptionOnFalseCondition()
    {
        // Arrange
        var condition = false;
        var exception = new ArgumentException("Invalid argument");

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => Check.MustBe(condition, () => exception));
        Assert.Equal("Invalid argument", ex.Message);
    }

    [Fact]
    public void MustBeArgumentNotNull_ShouldNotThrowExceptionOnNonNullArgument()
    {
        // Arrange
        var nonNullArg = new object();

        // Act & Assert
        Check.MustBeArgumentNotNull(nonNullArg, "nonNullArg");
        // No exception should be thrown, so no assertion is needed here.
    }

    [Fact]
    public void MustBeArgumentNotNull_ShouldThrowOnNullArgument()
    {
        // Arrange
        object? nullArg = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => Check.MustBeArgumentNotNull(nullArg, "nullArg"));
        Assert.Equal("nullArg", ex.ParamName);
    }
}