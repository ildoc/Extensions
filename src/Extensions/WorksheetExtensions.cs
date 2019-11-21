using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OfficeOpenXml;

namespace Extensions
{
    public static class WorksheetExtensions
    {
        public static Dictionary<string, int> HeaderToDictionary(this ExcelWorksheet worksheet, int headerRow) =>
            worksheet
                .Cells[headerRow, worksheet.Dimension.Start.Column, headerRow, worksheet.Dimension.End.Column]
                .Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .ToDictionary(x => x.Text, x => x.Start.Column);

        public static ExcelWorksheet FindWorksheet(this ExcelWorkbook workbook, string workseetName) =>
            workbook.Worksheets.FirstOrDefault(x => x.Name == workseetName) ?? throw new Exception($"Current workbook doesn't contains any sheet named '{workseetName}'");

        public static T Get<T>(this ExcelWorksheet worksheet, int row, int col)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                // Excel can store numers as strings and getting them directly as numbers can result in getting 0s
                var valueString = worksheet.GetValue<string>(row, col);
                if (string.IsNullOrEmpty(valueString))
                    return default;
                return (T)(object)Convert.ToInt32(valueString);
            }
            return worksheet.GetValue<T>(row, col);
        }

        public static Dictionary<string, int> WriteHeaders(this ExcelWorksheet currentWorksheet, int headerRow, params string[] headers)
        {
            var cols = headers.ToList();
            var colonne = cols.ToDictionary(x => x, x => cols.IndexOf(x) + 1);

            //scrivo gli header
            foreach (var h in colonne)
                currentWorksheet.Cells[headerRow, h.Value].Value = h.Key;

            return colonne;
        }

        public static void FormatDateColumns(this ExcelWorksheet currentWorksheet, Dictionary<string, int> columns, int startRow, params string[] dateCols) =>
            currentWorksheet.FormatDateColumns(columns, startRow, "mm-dd-yy", dateCols);

        public static void FormatDateColumns(this ExcelWorksheet currentWorksheet, Dictionary<string, int> columns, int startRow, string dateFormat, params string[] dateCols)
        {
            foreach (var dc in dateCols)
            {
                using var col = currentWorksheet.Cells[startRow, columns[dc], currentWorksheet.Dimension.End.Row, columns[dc]];
                col.Style.Numberformat.Format = dateFormat;
            }
        }

        public static bool RowIsBlank(this ExcelWorksheet ws, int row) =>
            ws.Cells[row, ws.Dimension.Start.Column, row, ws.Dimension.End.Column].All(x => x.Value == null);

        public static float GetFloat(this ExcelWorksheet worksheet, int row, int col)
        {
            var value = worksheet.Get<string>(row, col);
            if (string.IsNullOrEmpty(value))
                return 0;

            return float.Parse(value.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        }

    }
}
