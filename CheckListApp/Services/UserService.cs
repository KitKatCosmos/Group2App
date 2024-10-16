using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckListApp.Model;
using CheckListApp.Data;
using SQLite;
using System.Linq;

namespace CheckListApp.Services
{
    public class UserService
    {
        private readonly TaskDatabase _database;

        public UserService(TaskDatabase database)
        {
            _database = database;
        }

        public async Task<Users> GetUserAsync(int userId)
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            return await table.FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            return await table.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> GetFirstUserAsync()
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            return await table.FirstOrDefaultAsync();
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            return await table.ToListAsync();
        }

        public async Task<int> SaveUserAsync(Users user)
        {
            await _database.InitializeDatabaseAsync();
            if (user.UserID != 0)
            {
                user.UpdatedDate = DateTime.Now;
                return await _database.UpdateAsync(user);
            }
            else
            {
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                return await _database.InsertAsync(user);
            }
        }

        public async Task<int> DeleteUserAsync(Users user)
        {
            await _database.InitializeDatabaseAsync();
            return await _database.DeleteAsync(user);
        }

        public async Task<Users> AuthenticateUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user != null)
            {
                // In a real application, you should use proper password hashing and verification
                // This is a simplified example and should not be used in production
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await GetUserAsync(userId);
            if (user != null && user.Password == currentPassword)
            {
                user.Password = newPassword;
                user.UpdatedDate = DateTime.Now;
                await _database.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            var user = await GetUserAsync(userId);
            if (user != null)
            {
                user.Email = newEmail;
                user.UpdatedDate = DateTime.Now;
                await _database.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            var user = await table.FirstOrDefaultAsync(u => u.Username == username);
            return user != null;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            var user = await table.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }

        public async Task<int> GetUserCountAsync()
        {
            await _database.InitializeDatabaseAsync();
            var table = await _database.Table<Users>();
            return await table.CountAsync();
        }
    }
}