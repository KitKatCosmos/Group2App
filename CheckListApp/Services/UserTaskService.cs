using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CheckListApp.Model;
using CheckListApp.Data;

namespace CheckListApp.Services
{
    public class UserTaskService
    {
        private readonly TaskDatabase _database;

        public UserTaskService()
        {
            _database = TaskDatabase.Instance;
        }

        private async Task EnsureDatabaseInitializedAsync()
        {
            try
            {
                await _database.InitializeDatabaseAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database initialization failed: {ex.Message}");
                throw;
            }
        }

        public async Task<List<UserTask>> GetTasksForUserAsync(int userId)
        {
            await EnsureDatabaseInitializedAsync();
            try
            {
                var tasks = await _database.GetTasksForUserAsync(userId);
                Debug.WriteLine($"Retrieved {tasks.Count} tasks for user {userId}");
                return tasks;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTasksForUserAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UserTask> GetTaskAsync(int userId, int taskId)
        {
            await EnsureDatabaseInitializedAsync();
            try
            {
                Debug.WriteLine($"Attempting to retrieve task with ID: {taskId} for user ID: {userId}");
                var task = await _database.GetTaskAsync(userId, taskId);
                if (task != null)
                {
                    Debug.WriteLine($"Retrieved task: {task.Title} (ID: {task.TaskID}) for user ID: {task.UserId}");
                }
                else
                {
                    Debug.WriteLine($"No task found with ID {taskId} for user ID: {userId}");
                }
                return task;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTaskAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<int> SaveTaskAsync(UserTask task)
        {
            await EnsureDatabaseInitializedAsync();
            try
            {
                if (task.TaskID != 0)
                {
                    var result = await _database.UpdateAsync(task);
                    Debug.WriteLine($"Updated task {task.TaskID} for user {task.UserId}");
                    return result;
                }
                else
                {
                    var result = await _database.InsertAsync(task);
                    Debug.WriteLine($"Inserted new task for user {task.UserId}, new TaskID: {result}");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SaveTaskAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteTaskAsync(UserTask task)
        {
            await EnsureDatabaseInitializedAsync();
            try
            {
                var result = await _database.DeleteAsync(task);
                Debug.WriteLine($"Deleted task {task.TaskID} for user {task.UserId}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in DeleteTaskAsync: {ex.Message}");
                throw;
            }
        }
    }
}