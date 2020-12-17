using System;
using System.Collections.Generic;
using FluentAssertions;
using ReportReader.App;
using Xunit;

namespace ReportReader.Tests
{
    public class AppOptionsUnitTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { new[] {"--file", "TestFile.txt"}, "TestFile.txt", false, string.Empty },
                new object[] { new[] {"--file2", "TestFile.txt"}, string.Empty, false, string.Empty },
                new object[] { new[] {"--sortByStartDate" }, string.Empty, true, string.Empty },
                new object[] { new[] {"--sortByStartDate2" }, string.Empty, false, string.Empty },
                new object[] { new[] {"--project", "1" }, string.Empty, false, "1" },
                new object[] { new[] {"--project" }, string.Empty, false, string.Empty },
                new object[] { new[] {"--project2", "1" }, string.Empty, false, string.Empty },
                new object[] { new[] {"--file", "TestFile.txt", "--sortByStartDate", "--project", "1" }, "TestFile.txt", true, "1" },
                new object[] { new string[] {}, string.Empty, false, string.Empty}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void FromArguments_ValidArguments_ShouldReturnOptions(string[] args, string expectedPath, bool expectedSortByStartDate, string expectedProjectId)
        {
            // Act
            var appOptions = AppOptions.FromArguments(args);

            // Assert
            appOptions.Path.Should().Be(expectedPath);
            appOptions.SortByStartDate.Should().Be(expectedSortByStartDate);
            appOptions.ProjectId.Should().Be(expectedProjectId);
        }

        [Fact]
        public void FromArguments_Null_ShouldThrow()
        {
            // Arrange
            string[] args = null;

            // Act
            Action action = () => AppOptions.FromArguments(args);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
