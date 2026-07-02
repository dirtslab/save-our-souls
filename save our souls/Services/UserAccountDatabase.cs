using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace save_our_souls.Services
{
    public class UserAccountDatabase
    {
        private const int SaltSize = 16;      // 128-bit
        private const int HashSize = 32;      // 256-bit
        private const int Iterations = 100_000;

        SQLiteAsyncConnection? db;

        async Task Init()
        {
            if (db != null)
                return;
            db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await db.CreateTableAsync<Models.UserAccountModel>();
        }

        public async Task AddUserAccountAsync(Models.UserAccountModel userAccount)
        {
            await Init();
            if (await GetUserNameAsync(userAccount.Username) != null)
            {
                throw new Exception("Username already exists.");
            }
            userAccount.Password = HashPassword(userAccount.Password);
            await db.InsertAsync(userAccount);
        }

        public async Task<Models.UserAccountModel?> GetUserAccountAsync(string username, string password)
        {
            await Init();
            var userAccount = await db.Table<Models.UserAccountModel>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();

            if (userAccount == null)
                return null;

            if (VerifyPassword(password, userAccount.Password))
                return userAccount;

            return null;
        }

        public async Task<Models.UserAccountModel?> GetUserNameAsync(string username)
        {
            await Init();
            var userAccount = await db.Table<Models.UserAccountModel>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            return userAccount;
        }

        private static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 3)
                throw new FormatException("Unexpected hash format. Should be formatted as '{iterations}.{salt}.{hash}'");
            int iterations = Convert.ToInt32(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] hash = Convert.FromBase64String(parts[2]);
            byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                hash.Length);
            return CryptographicOperations.FixedTimeEquals(testHash, hash);
        }
    }
}
