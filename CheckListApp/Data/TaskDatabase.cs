using System;
using System.IO;
using SQLite;
using System.Threading.Tasks;
using CheckListApp.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace CheckListApp.Data
{
    public class TaskDatabase
    {
        private static TaskDatabase? _instance;
        private static readonly object _lock = new();
        private SQLiteAsyncConnection? _database;
        private const string DatabaseFilename = "checklist.db3";

        private bool _isInitialized = false;
        private readonly SemaphoreSlim _initializationLock = new SemaphoreSlim(1, 1);

        public static TaskDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new TaskDatabase();
                    }
                }
                return _instance;
            }
        }

        public TaskDatabase()
        {
            // Constructor is empty as initialization is done asynchronously
        }

        public async Task InitializeDatabaseAsync()
        {
            if (_isInitialized)
                return;

            await _initializationLock.WaitAsync();
            try
            {
                if (!_isInitialized)
                {
                    string dbPath = GetDatabasePath(DatabaseFilename);

                    // Delete existing database file
                    if (File.Exists(dbPath))
                    {
                        File.Delete(dbPath);
                    }

                    _database = new SQLiteAsyncConnection(dbPath);
                    await _database.CreateTableAsync<Users>();
                    await _database.CreateTableAsync<UserTask>();
                    await _database.CreateTableAsync<Comment>();
                    await _database.CreateTableAsync<Notification>();

                    _isInitialized = true;

                    // Perform test inserts only if the database is empty
                    if (await IsEmptyDatabase())
                    {
                        await PerformTestInsertsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
            }
            finally
            {
                _initializationLock.Release();
            }
        }

        private async Task<bool> IsEmptyDatabase()
        {
            var userCount = await _database.Table<Users>().CountAsync();
            return userCount == 0;
        }

        private async Task PerformTestInsertsAsync()
        {
            try
            {
                // Insert test user
                var user = new Users
                {
                    Username = "User1",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Password = "",
                    PasswordHash = new byte[0],
                    Email = "jdoe@it488.com",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                int userId = await InsertAsync(user);
                Debug.WriteLine($"Test user inserted with ID: {userId}");

                // Insert test task
                var task = new UserTask
                {
                    UserId = userId,
                    Title = "Testing",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Category = "Testing",
                    PriorityLevel = 3, // High priority
                    DueDate = DateTime.UtcNow.AddDays(10),
                    IsCompleted = false,
                    IsRepeating = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                int taskId = await InsertAsync(task);
                Debug.WriteLine($"Test task inserted with ID: {taskId} for user ID: {userId}");

                Debug.WriteLine($"Test user (ID: {userId}) and task (ID: {taskId}) have been inserted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in PerformTestInsertsAsync: {ex.Message}");
            }
        }

        public static string GetDatabasePath(string dbName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, dbName);
        }

        public async Task<AsyncTableQuery<T>> Table<T>() where T : new()
        {
            await InitializeDatabaseAsync();
            return _database.Table<T>();
        }

        public async Task<int> InsertAsync<T>(T entity) where T : new()
        {
            await InitializeDatabaseAsync();
            return await _database.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : new()
        {
            await InitializeDatabaseAsync();
            return await _database.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : new()
        {
            await InitializeDatabaseAsync();
            return await _database.DeleteAsync(entity);
        }

        public async Task<List<UserTask>> GetTasksForUserAsync(int userId, int limit = 100, int offset = 0)
        {
            await InitializeDatabaseAsync();
            try
            {
                return await _database.Table<UserTask>()
                                      .Where(t => t.UserId == userId)
                                      .Skip(offset)
                                      .Take(limit)
                                      .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTasksForUserAsync: {ex.Message}");
                return new List<UserTask>();
            }
        }

        public async Task<UserTask> GetTaskAsync(int userId, int taskId)
        {
            await InitializeDatabaseAsync();
            try
            {
                Debug.WriteLine($"Querying task with TaskID: {taskId} for UserID: {userId}");
                var task = await _database.Table<UserTask>()
                                          .Where(t => t.UserId == userId && t.TaskID == taskId)
                                          .FirstOrDefaultAsync();

                if (task != null)
                {
                    Debug.WriteLine($"Task retrieved: {task.Title} for UserID: {userId}");
                }
                else
                {
                    Debug.WriteLine($"No task found for TaskID: {taskId} and UserID: {userId}");
                }

                return task;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTaskAsync: {ex.Message}");
                return null;
            }
        }

    }
}