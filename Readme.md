# Report reader console application

A console application that reads an input data, transforms the data and finally outputs the results to a console.
This solution consists of:
- ReportReader.App project (.NET 5, a console application)
- ReportReader.Domain project (.NET 5, a library with domain logic)
- ReportReader.Tests project (.NET 5, xUnit unit tests)

## How to run

Build solution and run ReportReader.App.exe.

In the case if you do not have .NET 5, you can find more information here (https://dotnet.microsoft.com/download/dotnet/5.0). 
Also, you can downgrade the .NET framework version in the csproj files.

If you use Microsoft Visual Studio, you can set command line arguments in the `launchSettings.json`:
```
"ReportReaderApp": {
      "commandName": "Project",
      "commandLineArgs": "--file ExampleData.tsv"
    }
``` 

### Run options

The application supports the command line arguments:

|     Option              |  Mandatory |                    Description                        |
| ----------------------  | ---------- | :---------------------------------------------------: |
|--file \<path>           | yes        | full path to the input file                           |
|--sortByStartDate        | no         | sort results by column "Start date" in ascending order|
|--project \<project id>  | no         | filter results by column "Project"                    |


Example of usage: `ReportReader.App.exe --file inputFile.txt --sortByStartDate --project 1`

### Input data

Input data is tab-separated UTF-8 text with a specified header row (as in the example below).

| Project | Description   |	       Start date       |	Category   | Responsible | Savings amount |	Currency   | Complexity |
| ------- | -----------   | ----------------------- | ----------   | ----------- | -------------- |----------- | ---------- |
| 1       | Description 1 | 2014-01-01 00:00:00.000 |  Category1   | Person1     |	NULL          |	NULL       | Simple     |
| 2       | Description 2 | 2013-01-01 00:00:00.000 |  Category2   | Person2     | 141415.942696  |	EUR        | Moderate   |

Notes:
- Columns "Savings amount" and "Currency" can have missing values denoted as `NULL`.
- Dates (Start date) should conform to the format `yyyy-mm-dd hh:mm:ss.sss`.
- Money (Savings amount) values should be numbers with a point as the decimal separator.
- Column "Complexity" has a certain set of values (`Simple, Moderate, Hazardous`).
- Column "Currency" has a certain set of values (`EUR`).
- Lines that are empty or start with comment mark # are skipped.
- All column names, columns with set of values and `NULL` symbol are __case sensitive__.

## How to run unit tests

Build the test project `ReportReader.Tests` and run all test cases with your favorite runner (e.g. ReSharper).