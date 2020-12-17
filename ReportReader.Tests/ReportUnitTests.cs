using System;
using System.Collections.Generic;
using FluentAssertions;
using ReportReader.Domain;
using ReportReader.Domain.Common;
using Xunit;

namespace ReportReader.Tests
{
    public class ReportUnitTests
    {
        public static IEnumerable<object[]> ValidTestData =>
            new List<object[]>
            {
                new object[]
                {
                    // Valid
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                        "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple" },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                    $"6 Black and white logo paper 2012-06-01 00:00:00.000 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // Different header order
                    new [] { "Project	Responsible	Description	Complexity	Start date	Category	Savings amount	Currency",
                        "2	Daisy Milks	Harmonize Lactobacillus acidophilus sourcing	Simple	2014-01-01 00:00:00.000	Dairy	11689.322459	EUR" },

                    $"Project Responsible Description Complexity Start date Category Savings amount Currency{Environment.NewLine}" +
                    $"2 Daisy Milks Harmonize Lactobacillus acidophilus sourcing Simple 2014-01-01 00:00:00.000 Dairy 11689.322459 EUR{Environment.NewLine}"
                },
                new object[]
                {
                    // With NULL
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	NULL	NULL	Simple" },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With comments
                    new [] { "# This is comment",
                        "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "# This is comment",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                    },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With empty line
                    new [] { " ", "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity", " ",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple" },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With empty line
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity"},

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}"
                }
            };

        public static IEnumerable<object[]> InvalidTestData =>
            new List<object[]>
            {
                new object[]
                {
                    // With NULL value in non-nullable column
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	NULL	NULL	NULL	Simple"},

                    "Value can not be NULL for column Responsible"
                },
                new object[]
                {
                    // Not all columns in a header
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	NULL	NULL	NULL	Simple"},

                    "Column(s) Complexity not found"
                },
                new object[]
                {
                    // No header
                    new [] {"2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	NULL	NULL	NULL	Simple"},

                    "Column(s) Project, Description, Start date, Category, Responsible, Savings amount, Currency, Complexity not found"
                },
                new object[]
                {
                    // Wrong date format
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014/01/01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple"},

                    "Failed to parse date 2014/01/01 00:00:00.000"
                },
                new object[]
                {
                    // Wrong decimal format
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	money	EUR	Simple"},

                    "Failed to parse money value money"
                },
                new object[]
                {
                    // Wrong complexity format
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Super Simple"},

                    "Failed to parse enumeration value Super Simple"
                },
                new object[]
                {
                    // Missed value
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple"},

                    "Does not contain all values"
                },
                new object[]
                {
                    // Empty array
                    new string[] { },

                    "Failed to read a header"
                },
            };

        [Theory]
        [MemberData(nameof(ValidTestData))]
        public void ToString_ValidInput_ShouldReturnString(string[] text, string expected)
        {
            // Act
            Result<Report> reportResult = Report.FromText(text);

            // Assert
            reportResult.IsSuccessful.Should().BeTrue();
            reportResult.Value.ToString().Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(InvalidTestData))]
        public void FromText_InvalidInput_ShouldReturnUnsuccessful(string[] text, string error)
        {
            // Act
            Result<Report> reportResult = Report.FromText(text);

            // Assert
            reportResult.IsSuccessful.Should().BeFalse();
            reportResult.Error.Should().Contain(error);
        }

        [Fact]
        public void FilterByProjectId_ProjectId_ShouldReturnFiltered()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "3	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple",
                "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple"
            };

            string expected =
                $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                $"3 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}";

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            report.FilterByProject("3");

            // Assert
            report.ToString().Should().Be(expected);
        }

        [Fact]
        public void FilterByProjectId_EmptyProjectId_ShouldThrow()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
            };

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            Action action = () => report.FilterByProject("");

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void SortByStartDateAsc_ShouldReturnSorted()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2019-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "2	Harmonize Lactobacillus acidophilus sourcing	2015-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "3	Harmonize Lactobacillus acidophilus sourcing	2017-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "6	Black and white logo paper	2011-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple",
                "6	Black and white logo paper	2015-06-01 00:00:00.001	Office supplies	Clark Kent	4880.199567	EUR	Simple"
            };

            string expected =
                $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                $"6 Black and white logo paper 2011-06-01 00:00:00.000 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}" +
                $"2 Harmonize Lactobacillus acidophilus sourcing 2015-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                $"6 Black and white logo paper 2015-06-01 00:00:00.001 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}" +
                $"3 Harmonize Lactobacillus acidophilus sourcing 2017-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                $"2 Harmonize Lactobacillus acidophilus sourcing 2019-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}";

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            report.SortByStartDateAsc();

            // Assert
            report.ToString().Should().Be(expected);
        }
    }
}
