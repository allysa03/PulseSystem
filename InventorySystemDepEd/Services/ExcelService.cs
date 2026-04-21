using ClosedXML.Excel;
using InventorySystemDepEd.Shared.DTOs;
using InventorySystemDepEd.Shared.DTOs.Personnels;
using InventorySystemDepEd.Shared.DTOs.Positions;
using System.Net.Http.Json;

namespace InventorySystemDepEd.Services
{
    public class ExcelService
    {
        private readonly HttpClient _http;

        public ExcelService(HttpClient http)
        {
            _http = http;
        }

        // ===============================
        // GENERATE TEMPLATE (API DTO)
        // ===============================
        public byte[] GenerateTemplate(List<PositionDto> positions, List<OfficeDto> offices)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("PersonnelTemplate");

            // HEADER
            var headerRange = worksheet.Range("A1:I1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Fill.BackgroundColor = XLColor.DarkGreen;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            worksheet.Cell(1, 1).Value = "EmployeeID";
            worksheet.Cell(1, 2).Value = "FirstName";
            worksheet.Cell(1, 3).Value = "MiddleName";
            worksheet.Cell(1, 4).Value = "LastName";
            worksheet.Cell(1, 5).Value = "Position";
            worksheet.Cell(1, 6).Value = "Office";
            worksheet.Cell(1, 7).Value = "EmailAddress";
            worksheet.Cell(1, 8).Value = "ContactNumber";
            worksheet.Cell(1, 9).Value = "HiredDate";

            worksheet.Range("A1:I1000").SetAutoFilter();

            // ===============================
            // Hidden Positions Sheet
            // ===============================
            var posSheet = workbook.Worksheets.Add("Positions");
            for (int i = 0; i < positions.Count; i++)
                posSheet.Cell(i + 1, 1).Value = positions[i].PositionName;

            posSheet.Hide();

            // ===============================
            // Hidden Offices Sheet
            // ===============================
            var officeSheet = workbook.Worksheets.Add("Offices");
            for (int i = 0; i < offices.Count; i++)
                officeSheet.Cell(i + 1, 1).Value = offices[i].OfficeName;

            officeSheet.Hide();

            int lastRow = 1000;

            for (int i = 2; i <= lastRow; i++)
            {
                // Position dropdown
                var posValidation = worksheet.Cell(i, 5).CreateDataValidation();
                posValidation.List($"=Positions!$A$1:$A${positions.Count}");

                // Office dropdown
                var officeValidation = worksheet.Cell(i, 6).CreateDataValidation();
                officeValidation.List($"=Offices!$A$1:$A${offices.Count}");

                // Date validation
                var dateValidation = worksheet.Cell(i, 9).CreateDataValidation();
                dateValidation.Date.Between(new DateTime(1990, 1, 1), DateTime.Today);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        // ===============================
        // IMPORT PERSONNEL (API DTO)
        // ===============================
        public async Task<List<PersonnelDto>> ImportPersonnelAsync(Stream fileStream)
        {
            var positions = await _http.GetFromJsonAsync<List<PositionDto>>("api/positions") ?? new();
            var offices = await _http.GetFromJsonAsync<List<OfficeDto>>("api/offices") ?? new();

            var result = new List<PersonnelDto>();

            using var ms = new MemoryStream();
            await fileStream.CopyToAsync(ms);
            ms.Position = 0;

            using var workbook = new XLWorkbook(ms);
            var sheet = workbook.Worksheet(1);

            int rowCount = sheet.LastRowUsed()?.RowNumber() ?? 1;

            for (int row = 2; row <= rowCount; row++)
            {
                var employeeId = sheet.Cell(row, 1).GetString();
                if (string.IsNullOrWhiteSpace(employeeId))
                    continue;

                var positionName = sheet.Cell(row, 5).GetString();
                var officeName = sheet.Cell(row, 6).GetString();

                var position = positions.FirstOrDefault(x => x.PositionName == positionName);
                var office = offices.FirstOrDefault(x => x.OfficeName == officeName);

                DateTime? hiredDate = sheet.Cell(row, 9).GetValue<DateTime?>();

                result.Add(new PersonnelDto
                {
                    EmployeeID = employeeId,
                    FirstName = ToProperCase(sheet.Cell(row, 2).GetString()),
                    MiddleName = ToProperCase(sheet.Cell(row, 3).GetString()),
                    LastName = ToProperCase(sheet.Cell(row, 4).GetString()),
                    PositionId = position?.PositionId,
                    OfficeId = office?.OfficeId,
                    EmailAddress = sheet.Cell(row, 7).GetString(),
                    ContactNumber = sheet.Cell(row, 8).GetString(),
                    HiredDate = hiredDate
                });
            }

            return result;
        }

        // ===============================
        // UTIL
        // ===============================
        private string ToProperCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return string.Join(" ",
                input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                     .Select(w => char.ToUpper(w[0]) + w[1..].ToLower()));
        }
    }
}