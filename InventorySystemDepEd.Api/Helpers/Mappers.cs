
using InventorySystemDepEd.Shared.DTOs.Positions;
using System.Security.Cryptography;
using System.Text;

namespace InventorySystemDepEd.Helpers
{
    public static class Mapper
    {
        // ✅ MD5 helper
        private static string ComputeMD5(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        // -------------------------------
        // Map UserDto + Personnel -> UsersModel
        // -------------------------------
        //public static UsersModel MapToUser(UserDto dto, PersonnelsModel personnel)
        //{
        //    return new UsersModel
        //    {
        //        UserEmailAddress = dto.Email,
        //        UserPassword = ComputeMD5(dto.Password), // MD5 hash
        //        UserRole = dto.Role,
        //        Personnel = personnel,  // navigation property
        //        UserStatus = "Pending",
        //        TermAndCon = false,
        //        UserIsApproved = false,
        //        UserLastLog = DateTime.Now
        //    };
        //}

        // -------------------------------
        // Map PersonnelDto -> PersonnelsModel
        // -------------------------------
        //public static PersonnelsModel MapToPersonnel(PersonnelDto dto, UsersModel user = null)
        //{
        //    var personnel = new PersonnelsModel
        //    {
        //        EmployeeID = dto.EmployeeID,
        //        FirstName = dto.FirstName,
        //        MiddleName = dto.MiddleName,
        //        LastName = dto.LastName,
        //        EmailAddress = dto.EmailAddress
        //    };

        //    // Link the User's ID to AccountID if a User is provided
        //    if (user != null)
        //    {
        //        personnel.AccountID = user.UserId;
        //        personnel.User = user; // optional navigation
        //    }

        //    return personnel;
        //}

        // -------------------------------
        // Map SupplierDto -> SuppliersModel
        // -------------------------------
        //public static SuppliersModel MapToSupplier(SupplierDto dto)
        //{
        //    return new SuppliersModel
        //    {
        //        SupplierName = dto.SupplierName,
        //        SupplierAddress = dto.SupplierAddress,
        //        SupplierContactNumber = dto.SupplierContactNumber,
        //        SupplierEmailAddress = dto.SupplierEmailAddress,
        //        SupplierTinNumber = dto.SupplierTinNumber
        //    };
        //}

        // -------------------------------
        // Map PositionsModel -> PositionDto
        // -------------------------------
        //public static List<PositionDto> MapPositions(List<PositionsModel> positions)
        //{
        //    return positions.Select(p => new PositionDto
        //    {
        //        PositionId = p.PositionId,
        //        PositionName = p.PositionName
        //    }).ToList();
        //}
    }
}