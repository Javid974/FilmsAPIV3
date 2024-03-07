using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ExcelFileParser
    {
        public List<Movie> ParseExcelFile(Stream fileStream)
        {
            using (XLWorkbook workbook = new XLWorkbook(fileStream))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                return ParseWorksheet(worksheet);
            }
        }

        private List<Movie> ParseWorksheet(IXLWorksheet worksheet)
        {
            var movies = new List<Movie>();

            // Skip the first row (header)
            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var movie = ParseRow(row);
                if (movie != null)
                {
                    int viewingDate;
                    if (int.TryParse(worksheet.Name, out viewingDate))
                    {
                        movie.ViewingYear = int.Parse(worksheet.Name);
                    }
                    else
                    {
                        throw new Exception($"Invalid worksheet name: {worksheet.Name}");
                    }

                    movies.Add(movie);
                }
            }

            return movies;
        }

        private Movie? ParseRow(IXLRow row)
        {
            if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
            {
                return null;
            }
            var movie = new Movie()
            {
                ViewingDate = row.Cell(1).GetValue<DateTime>().ToUniversalTime(),
                // Assume title is in the first column (A)
                Title = row.Cell(2).Value.ToString(),
                Directors = new List<Director>(),
                // Assume director is in the second column (B)
                // Assume support is in the third column (C)


                // Etc.
            };
            var support = row.Cell(4).Value.ToString();
            if (support != null && support != string.Empty)
            {
                if (support == "Blue Ray" || support == "Blue ray")
                {
                    support = "BluRay";
                }
                if (support == "DIV X")
                {
                    support = "DIVX";
                }
                if (Enum.TryParse<Support>(support, true, out Support parsedSupport)) // ignoreCase is set to true
                {
                    movie.Support = parsedSupport;
                }
            }

            var directorsCellValue = row.Cell(3).Value.ToString();
            var directorNames = directorsCellValue.Split(',');
            movie.Directors.AddRange(directorNames.Select(name => new Director { Name = name.Trim() }));

            return movie;
        }
    }
}
