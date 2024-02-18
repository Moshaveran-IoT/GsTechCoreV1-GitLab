using System.Text;

using Moshaveran.Library;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Helpers))]
[Trait("Category", nameof(StringHelper))]
public sealed class StringHelperTests
{
    [Fact]
    public void AppendAll_AppendsEmptyLinesWhenLinesContainsEmptyStrings()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = new List<string> { "", "line2", "" };

        // Act
        var result = sb.AppendAll(lines);

        // Assert
        _ = result.ToString().Should().Be("line2");
    }

    [Fact]
    public void AppendAll_AppendsNonEmptyLinesWhenLinesContainsNonEmptyStrings()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = new List<string> { "line1", "line2" };

        // Act
        var result = sb.AppendAll(lines);

        // Assert
        _ = result.ToString().Should().Be("line1line2");
    }

    [Fact]
    public void AppendAll_DoesNothingWhenLinesIsEmpty()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = Enumerable.Empty<string>();

        // Act
        var result = sb.AppendAll(lines);

        // Assert
        _ = result.Should().BeEquivalentTo(new StringBuilder());
    }

    [Fact]
    public void AppendAll_DoesNothingWhenLinesIsNull()
    {
        // Arrange
        var sb = new StringBuilder();
        IEnumerable<string>? lines = null;

        // Act
        var result = sb.AppendAll(lines!);

        // Assert
        _ = result.Should().BeEquivalentTo(new StringBuilder());
    }

    [Fact]
    public void AppendAll_ThrowsArgumentNullExceptionWhenStringBuilderIsNull()
    {
        // Arrange
        StringBuilder? sb = null;
        var lines = new List<string> { "line1", "line2" };

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => sb!.AppendAll(lines));
    }

    [Fact]
    public void AppendAllLine_AppendsEmptyLinesWhenLinesContainsEmptyStrings()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = new List<string> { "", "line2", "" };

        // Act
        var result = sb.AppendAllLine(lines);

        // Assert
        _ = result.ToString().Should().Be("\r\nline2\r\n\r\n");
    }

    [Fact]
    public void AppendAllLine_AppendsNonEmptyLinesWhenLinesContainsNonEmptyStrings()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = new List<string> { "line1", "line2" };

        // Act
        var result = sb.AppendAllLine(lines);

        // Assert
        _ = result.ToString().Should().Be("line1\r\nline2\r\n");
    }

    [Fact]
    public void AppendAllLine_DoesNothingWhenLinesIsEmpty()
    {
        // Arrange
        var sb = new StringBuilder();
        var lines = Enumerable.Empty<string>();

        // Act
        var result = sb.AppendAllLine(lines);

        // Assert
        _ = result.Should().BeEquivalentTo(new StringBuilder());
    }

    [Fact]
    public void AppendAllLine_DoesNothingWhenLinesIsNull()
    {
        // Arrange
        var sb = new StringBuilder();
        IEnumerable<string>? lines = null;

        // Act
        var result = sb.AppendAllLine(lines!);

        // Assert
        _ = result.Should().BeEquivalentTo(new StringBuilder());
    }

    [Fact]
    public void AppendAllLine_ThrowsArgumentNullExceptionWhenStringBuilderIsNull()
    {
        // Arrange
        StringBuilder? sb = null;
        var lines = new List<string> { "line1", "line2" };

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => sb!.AppendAllLine(lines));
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrueWhenStringIsNull()
    {
        // Arrange
        string? s = null;

        // Act
        var result = s.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrueWhenStringIsEmpty()
    {
        // Arrange
        string s = string.Empty;

        // Act
        var result = s.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsFalseWhenStringIsNotEmpty()
    {
        // Arrange
        string s = "test";

        // Act
        var result = s.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }
}