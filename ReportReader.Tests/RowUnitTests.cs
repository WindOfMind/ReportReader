using System;
using FluentAssertions;
using ReportReader.Domain;
using ReportReader.Domain.Columns;
using ReportReader.Domain.Enumerations;
using Xunit;

namespace ReportReader.Tests
{
    public class RowUnitTests
    {
        [Fact]
        public void Row_FromText_Text_ShouldReturnRow()
        {
            // Arrange
            string line = "2012-06-01 00:00:00.000	Office supplies	4880.199567	Simple";
            var columns = new Column[]
            {
                new DateColumn("Date Column"),
                new StringColumn("String Column"),
                new MoneyColumn("Money Column"),
                new EnumerationColumn("Enumeration Column", Complexity.AllComplexities)
            };

            // Act
            var sut = Row.FromLine(line, columns, '\t');
            var result = sut.Value.ToString();

            // Assert
            sut.IsSuccessful.Should().BeTrue();
            result.Should().Be("2012-06-01 00:00:00.000 Office supplies 4880.199567 Simple");
        }

        [Fact]
        public void Row_FromText_MissingColumn_ShouldReturnError()
        {
            // Arrange
            string line = "2012-06-01 00:00:00.000	Office supplies	Simple";
            var columns = new Column[]
            {
                new DateColumn("Date Column"),
                new StringColumn("String Column"),
                new MoneyColumn("Money Column"),
                new EnumerationColumn("Enumeration Column", Complexity.AllComplexities)
            };

            // Act
            var sut = Row.FromLine(line, columns, '\t');

            // Assert
            sut.IsSuccessful.Should().BeFalse();
            sut.Error.Should().Contain("Does not contain all values");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Row_FromText_EmptyOrNull_ShouldThrow(string input)
        {
            // Arrange
            var columns = new Column[]
            {
                new DateColumn("Date Column"),
                new StringColumn("String Column"),
                new MoneyColumn("Money Column"),
                new EnumerationColumn("Enumeration Column", Complexity.AllComplexities)
            };

            // Act
            Action action = () => Row.FromLine(input, columns, '\t');

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
