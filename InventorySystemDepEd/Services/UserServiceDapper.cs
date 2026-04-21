using System.Data;
using System.Security.Cryptography;
using System.Text;
using Dapper;
//using InventorySystemDepEd.Models;

namespace InventorySystemDepEd.Services
{
    public class UserServiceDapper
    {
        //private readonly IDbConnection _connection;

        //public UserServiceDapper(IDbConnection connection)
        //{
        //    _connection = connection;
        //}

        //// Helper to compute MD5 hash
        //private static string ComputeMD5(string input)
        //{
        //    using var md5 = MD5.Create();
        //    var inputBytes = Encoding.UTF8.GetBytes(input);
        //    var hashBytes = md5.ComputeHash(inputBytes);
        //    var sb = new StringBuilder();
        //    foreach (var b in hashBytes)
        //    {
        //        sb.Append(b.ToString("x2"));
        //    }
        //    return sb.ToString();
        //}

        //public UsersModel? CreateUserIfPersonnelExists(string email, string password, int? role = null)
        //{
        //    _connection.Open();
        //    try
        //    {
        //        // Check if personnel exists
        //        var personnel = _connection.QueryFirstOrDefault<PersonnelsModel>(
        //            "SELECT * FROM tbl_personnels WHERE emailAddress = @Email",
        //            new { Email = email }
        //        );
        //        if (personnel == null) return null;

        //        // Check if user already exists
        //        var existingUser = _connection.QueryFirstOrDefault<UsersModel>(
        //            "SELECT * FROM tbl_users WHERE personnel_id = @PersonnelId",
        //            new { PersonnelId = personnel.PersonnelId }
        //        );
        //        if (existingUser != null) return existingUser;

        //        // Hash password using MD5
        //        var hashedPassword = ComputeMD5(password);

        //        // Insert user
        //        var user = _connection.QueryFirstOrDefault<UsersModel>(
        //            @"INSERT INTO tbl_users
        //                (user_emailAddress, user_password, user_role, personnel_id, user_status, termAndCon, user_IsApproved, user_lastLog)
        //              VALUES
        //                (@Email, @Password, @Role, @PersonnelId, 'Pending', false, false, CURRENT_TIMESTAMP)
        //              RETURNING *",
        //            new { Email = email, Password = hashedPassword, Role = role, PersonnelId = personnel.PersonnelId }
        //        );

        //        if (user != null) user.Personnel = personnel;
        //        return user;
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //}
    }
}