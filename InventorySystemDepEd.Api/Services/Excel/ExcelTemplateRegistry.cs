using InventorySystemDepEd.Shared.Excel;
using InventorySystemDepEd.Shared.Excel.Definitions;

namespace InventorySystemDepEd.Api.Services.Excel
{
    public class ExcelTemplateRegistry : IExcelTemplateRegistry
    {
        private readonly PositionProvider _positions;
        private readonly OfficeProvider _offices;

        public ExcelTemplateRegistry(
            PositionProvider positions,
            OfficeProvider offices)
        {
            _positions = positions;
            _offices = offices;
        }

        public ExcelTemplateDefinition Get(string name)
        {
            name = name.ToLower();

            if (name == "personnel")
            {
                return new ExcelTemplateDefinition
                {
                    Name = "personnel",
                    SheetName = "PersonnelTemplate",
                    DisplayName = "Personnel",

                    Fields = new List<ExcelFieldDefinition>
                    {
                        new() { Order = 1, Header = "EmployeeID", IsRequired = true, PropertyName = "EmployeeID" },
                        new() { Order = 2, Header = "FirstName", IsRequired = true, PropertyName = "FirstName" },
                        new() { Order = 3, Header = "MiddleName", IsRequired = true, PropertyName = "MiddleName" },
                        new() { Order = 4, Header = "LastName", IsRequired = true, PropertyName = "LastName" },
                        new() { Order = 5, Header = "EmailAddress", IsRequired = true, PropertyName = "EmailAddress" },
                        new() { Order = 6, Header = "ContractNumber", IsRequired = true, PropertyName = "ContractNumber" },
                        new()
                        {
                            Order = 7,
                            Header = "Position",
                            IsDropdown = true,
                            LookupSheet = "Positions",
                            PropertyName = "PositionId"
                        },

                        new()
                        {
                            Order = 8,
                            Header = "Office",
                            IsDropdown = true,
                            LookupSheet = "Offices",
                            PropertyName = "OfficeId"
                        },

                        new() { Order = 9, Header = "HiredDate", IsRequired = true, PropertyName = "HiredDate" },

                    }
                };
            }

            throw new Exception($"Template '{name}' not found");
        }

        public Dictionary<string, List<string>> GetDropdowns(string name)
        {
            name = name.ToLower();

            if (name == "personnel")
            {
                return new Dictionary<string, List<string>>
                {
                    { "Positions", _positions.GetPositions() },
                    { "Offices", _offices.GetOffices() }
                };
            }

            return new();
        }

        public List<ExcelTemplateInfo> GetAll()
        {
            return new()
            {
                new() { Name = "personnel", DisplayName = "Personnel" }
            };
        }
    }
}
