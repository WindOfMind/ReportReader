﻿using System;
using System.Collections.Generic;
using System.Linq;
using ReportReader.Domain.Columns;
using ReportReader.Domain.Common;

namespace ReportReader.Domain
{
    internal class Header
    {
        private readonly List<Column> _columns;

        private Header(IEnumerable<Column> columns)
        {
            _columns = columns.ToList();
        }

        public IReadOnlyCollection<Column> Columns => _columns;

        public static Result<Header> FromString(string headerString, Dictionary<string, Column> allColumns, char columnSeparator)
        {
            if (headerString == null)
                throw new ArgumentNullException(nameof(headerString));

            if (allColumns == null)
                throw new ArgumentNullException(nameof(allColumns));

            string[] columnNames = headerString.Split(columnSeparator);
            List<Column> columns = new List<Column>();

            foreach (string name in columnNames)
            {
                if (allColumns.ContainsKey(name))
                {
                    columns.Add(allColumns[name]);
                }
            }

            if (columns.Count != allColumns.Count)
            {
                IEnumerable<string> missedColumns = allColumns
                    .Keys
                    .Except(columns.Select(c => c.Name));

                return new Result<Header>($"Column(s) {string.Join(", ", missedColumns)} not found");
            }

            return new Result<Header>(new Header(columns));
        }

        public override string ToString()
        {
            return string.Join(" ", _columns);
        }
    }
}