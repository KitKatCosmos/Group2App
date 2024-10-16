using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CheckListApp.Data;
using CheckListApp.Model;

namespace CheckListApp.Services
{
    public class AuthenticationService
    {
        private readonly TaskDatabase _database;

        public AuthenticationService(TaskDatabase database)
        {
            _database = database;
        }

        public async Task<(Users User, List<UserTask> Tasks)> GetUserAndTasksAsync(int userId = 1)
        {
            try
            {
                await _database.InitializeDatabaseAsync();

                var userTable = await _database.Table<Users>();
                var user = await userTable
                    .Where(u => u.UserID == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception($"User with ID {userId} not found.");
                }

                var tasks = await _database.GetTasksForUserAsync(userId);

                return (user, tasks);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserAndTasksAsync: {ex.Message}");
                throw;
            }
        }
    }
}