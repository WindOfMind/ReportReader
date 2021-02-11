using FluentAssertions;
using ReportReader.Domain.Columns;
using ReportReader.Domain.Enumerations;
using Xunit;

namespace ReportReader.Tests.Columns
{
    public class EnumerationColumnUnitTests
    {
        [Fact]
        public void EnumerationColumn_Parse_ValidValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new EnumerationColumn("EnumerationColumn", Complexity.AllComplexities, false);

            // Act
            var result = sut.Parse(Complexity.Hazardous.ToString());

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [InlineData("unknown enumeration")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("NULL")]
        public void EnumerationColumn_Parse_InvalidValue_ShouldReturnError(string enumeration)
        {
            // Arrange
            var sut = new EnumerationColumn("EnumerationColumn", Complexity.AllComplexities, false);

            // Act
            var result = sut.Parse(enumeration);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.Error.Should().Be($"Failed to parse enumeration value {enumeration} in the column EnumerationColumn");
        }

        [Fact]
        public void NullableEnumerationColumn_Parse_NullValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new EnumerationColumn("EnumerationColumn", Complexity.AllComplexities, true);

            // Act
            var result = sut.Parse("NULL");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}
