//using InventorySystemDepEd.Data;
//using InventorySystemDepEd.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InventorySystemDepEd.Services
{
    public class UserServiceEfCore
    {
        //private readonly AppDbContext _context;

        //public UserServiceEfCore(AppDbContext context)
        //{
        //    _context = context;
        //}

        //public enum RegisterResult
        //{
        //    Success,
        //    PersonnelNotFound,
        //    AlreadyRegistered,
        //    Error
        //}

        //public async Task<(RegisterResult Result, UsersModel? User)> RegisterAsync(string email, string password)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        //            return (RegisterResult.Error, null);

        //        // Normalize email
        //        email = email.Trim().ToLower();

        //        // 1️⃣ Find personnel by email (case-insensitive, robust)
        //        var personnel = await _context.Personnels
        //            .FirstOrDefaultAsync(p => !string.IsNullOrWhiteSpace(p.EmailAddress) &&
        //                                      EF.Functions.ILike(p.EmailAddress, email));

        //        if (personnel == null)
        //        {
        //            Console.WriteLine($"[Debug] Personnel with email '{email}' not found.");
        //            return (RegisterResult.PersonnelNotFound, null);
        //        }

        //        // 2️⃣ Check if user already exists
        //        var existingUser = await _context.Users
        //            .FirstOrDefaultAsync(u => u.UserId == personnel.AccountID);

        //        if (existingUser != null)
        //            return (RegisterResult.AlreadyRegistered, null);

        //        // 3️⃣ Create new user
        //        var user = new UsersModel
        //        {
        //            UserEmailAddress = email,
        //            UserPassword = ComputeMD5(password),
        //            UserRole = 5,
        //            UserIsApproved = false,
        //            UserIsLocked = false,
        //            UserIsDeleted = false,
        //            UserLogAttempt = 0,
        //            UserCreatedAt = DateTime.UtcNow,
        //            UserApprovedAt = null,
        //            UserLastLog = null,
        //            UserModifiedAt = null,
        //            UserStatus = "Pending",
        //            TermAndCon = false
        //        };

        //        // Add user to context
        //        // 1️⃣ Insert the new user
        //        _context.Users.Add(user);
        //        await _context.SaveChangesAsync(); // now user.UserId is generated

        //        // 2️⃣ Update personnel (no Update() needed)
        //        personnel.AccountID = user.UserId;

        //        // 3️⃣ Save changes once
        //        await _context.SaveChangesAsync();

        //        Console.WriteLine($"[Success] User '{email}' registered with UserId={user.UserId}.");
        //        return (RegisterResult.Success, user);
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        Console.WriteLine($"[DB Error] Registration failed for '{email}': {dbEx}");
        //        if (dbEx.InnerException != null)
        //            Console.WriteLine($"[Inner Exception]: {dbEx.InnerException}");
        //        return (RegisterResult.Error, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"[Error] Registration failed for '{email}': {ex}");
        //        if (ex.InnerException != null)
        //            Console.WriteLine($"[Inner Exception]: {ex.InnerException}");
        //        return (RegisterResult.Error, null);
        //    }
        //}

        //public enum LoginResult
        //{
        //    Success,
        //    NotFound,
        //    NoPersonnelLinked,
        //    InvalidPassword,
        //    Locked,
        //    NotApproved
        //}

        //public async Task<(LoginResult Result, UsersModel? User)> LoginAsync(string email, string password)
        //{
        //    email = email.Trim().ToLower();

        //    var user = await _context.Users
        //        .Include(u => u.Personnel)
        //            .ThenInclude(p => p.Office)
        //                .ThenInclude(o => o.Department)
        //        .Include(u => u.Role)
        //        .FirstOrDefaultAsync(u => u.UserEmailAddress.ToLower() == email);

        //    if (user == null) return (LoginResult.NotFound, null);
        //    if (user.Personnel == null) return (LoginResult.NoPersonnelLinked, null);
        //    if (!user.UserIsApproved) return (LoginResult.NotApproved, null);
        //    if (user.UserIsLocked) return (LoginResult.Locked, null);

        //    string hashedInput = ComputeMD5(password);
        //    if (hashedInput != user.UserPassword)
        //    {
        //        await IncrementLogAttemptAsync(email);
        //        return (LoginResult.InvalidPassword, null);
        //    }

        //    // Successful login
        //    user.UserLogAttempt = 0;
        //    user.UserIsLocked = false;
        //    user.UserLastLog = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();

        //    return (LoginResult.Success, user);
        //}

        //private string ComputeMD5(string input)
        //{
        //    using var md5 = MD5.Create();
        //    var bytes = Encoding.UTF8.GetBytes(input);
        //    var hash = md5.ComputeHash(bytes);

        //    var sb = new StringBuilder();
        //    foreach (var b in hash)
        //        sb.Append(b.ToString("x2"));

        //    return sb.ToString();
        //}

        //// Retained IncrementLogAttemptAsync function
        //public async Task<bool> IncrementLogAttemptAsync(string email)
        //{
        //    email = email.Trim().ToLower();
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmailAddress.ToLower() == email);
        //    if (user == null) return false;

        //    user.UserLogAttempt++;
        //    if (user.UserLogAttempt >= 5)
        //        user.UserIsLocked = true;

        //    user.UserLastLog = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();

        //    return user.UserIsLocked;
        //}

        //public async Task ResetLogAttemptAsync(string email)
        //{
        //    email = email.Trim().ToLower();
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmailAddress.ToLower() == email);
        //    if (user == null) return;

        //    user.UserLogAttempt = 0;
        //    user.UserIsLocked = false;
        //    user.UserLastLog = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateLastLogAsync(string email)
        //{
        //    email = email.Trim().ToLower();
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmailAddress.ToLower() == email);
        //    if (user == null) return;

        //    user.UserLastLog = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<UsersModel?> GetUserWithDetailsAsync(int userId)
        //{
        //    return await _context.Users
        //        .Include(u => u.Role)
        //        .Include(u => u.Personnel)
        //            .ThenInclude(p => p.Position)
        //        .Include(u => u.Personnel)
        //            .ThenInclude(p => p.Office)
        //                .ThenInclude(o => o.Department)
        //        .FirstOrDefaultAsync(u => u.UserId == userId && !u.UserIsDeleted);
        //}

        //public async Task UpdateUserAsync(UsersModel user)
        //{
        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync();
        //}
    }
}