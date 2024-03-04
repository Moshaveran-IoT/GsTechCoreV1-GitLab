using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Validations;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Validations))]
[Trait("Category", nameof(Check))]
public sealed class CheckTests
{
    [Fact]
    public void ArgumentNotNull_ShouldReturnArgumentOnNonNullArgument()
    {
        // Arrange
        const string nonNullArg = "Non-null value";

        // Act
        var result = nonNullArg.ArgumentNotNull();

        // Assert
        Assert.Equal("Non-null value", result);
    }

    [Fact]
    public void ArgumentNotNull_ShouldThrowOnNullArgument()
    {
        // Arrange
        const string? nullArg = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => nullArg.ArgumentNotNull());
        Assert.Equal("nullArg", ex.ParamName);
    }

    [Fact]
    public void If_Returns_Fail_With_False_Value_When_Ok_Is_False()
    {
        // Arrange
        const bool ok = false;
        const int trueValue = 1;
        const int falseValue = 0;

        // Act
        var result = Check.If(ok, () => trueValue, () => falseValue);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.Equal(falseValue, result.Value);
    }

    [Fact]
    public void If_Returns_Failed_When_Ok_Is_False()
    {
        // Arrange
        const bool ok = false;

        // Act
        var result = Check.If(ok);

        // Assert
        Assert.False(result.IsSucceed);
    }

    [Fact]
    public void If_Returns_Succeed_When_Ok_Is_True()
    {
        // Arrange
        const bool ok = true;

        // Act
        var result = Check.If(ok);

        // Assert
        Assert.True(result.IsSucceed);
    }

    [Fact]
    public void If_Returns_Success_With_True_Value_When_Ok_Is_True()
    {
        // Arrange
        const bool ok = true;
        const int trueValue = 1;
        const int falseValue = 0;

        // Act
        var result = Check.If(ok, () => trueValue, () => falseValue);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.Equal(trueValue, result.Value);
    }

    [Fact]
    public void MustBe_Generic_ShouldNotThrowExceptionOnTrueCondition()
    {
        // Arrange
        const bool condition = true;

        // Act & Assert
        Check.MustBe<GsTechException>(condition);
        // No exception should be thrown, so no assertion is needed here.
    }

    [Fact]
    public void MustBe_Generic_ShouldThrowSpecificExceptionOnFalseCondition()
    {
        // Arrange
        const bool condition = false;

        // Act & Assert
        var ex = Assert.Throws<GsTechException>(() => Check.MustBe<GsTechException>(condition));
        // Additional assertions can be made here if needed, such as checking the exception message.
    }

    [Fact]
    public void MustBe_ShouldNotThrowExceptionOnTrueCondition()
    {
        // Arrange
        const bool condition = true;
        var exception = new ArgumentException("Invalid argument");

        // Act & Assert
        Check.MustBe(condition, () => exception);
        // No exception should be thrown, so no assertion is needed here.
    }

    [Fact]
    public void MustBe_ShouldThrowSpecificExceptionOnFalseCondition()
    {
        // Arrange
        const bool condition = false;
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