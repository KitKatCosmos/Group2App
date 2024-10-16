using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using CheckListApp.Services;
using Microsoft.VisualBasic;
using System.Formats.Tar;
using Microsoft.EntityFrameworkCore.Update;
using CheckListApp.Model;

namespace CheckListApp.View
{
    [QueryProperty(nameof(TaskId), "id")]
    [QueryProperty(nameof(UserId), "userId")]
    public partial class ItemDetailPage : ContentPage
    {
        private readonly UserTaskService _userTaskService;

        const string TaskKey = "savedTask";
        const string DescriptionKey = "savedDecription";
        const string PriorityKey = "savedPriority";
        const string DueDateKey = "savedDueDate";
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public bool IsEditMode { get; set; }

        public ItemDetailPage()
        {
            InitializeComponent();
            LoadSavedData();
            _userTaskService = new UserTaskService(); // Initialize the service to load the task details
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Log the passed TaskId and UserId
            Debug.WriteLine($"Navigated with TaskID: {TaskId} and UserID: {UserId}");

            // Load the task details using TaskId and UserId
            LoadTask(TaskId);
        }

        private async void LoadTask(int taskId)
        {
            Debug.WriteLine($"Attempting to load task with TaskID: {taskId} and UserID: {UserId}");

            try
            {
                // Retrieve the task using the UserTaskService
                var task = await _userTaskService.GetTaskAsync(UserId, taskId);

                if (task != null)
                {
                    Title = task.Title;
                    TitleLabel.Text = task.Title;
                    TaskEntry.Text = task.CreatedTask;
                    DescriptionEntry.Text = task.Description;
                    PriorityEntry.Text = $"Priority: {task.PriorityLevel}";
                    DueDateEntry.Text = $"Due Date: {task.DueDate.ToShortDateString()}";
                    IsCompletedCheckBox.IsChecked = task.IsCompleted;

                    // Make task details visible
                    TaskDetailContent.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Error", "Task not found.", "OK");
                    Debug.WriteLine($"No task found for TaskID: {taskId} and UserID: {UserId}");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading task details: {ex.Message}");
                await DisplayAlert("Error", "Failed to load task details.", "OK");
            }
        }

        private void LoadSavedData()
        {
            // Load saved data when the page is initialized
            TaskEntry.Text = Preferences.Get(TaskKey, string.Empty);
            DescriptionEntry.Text = Preferences.Get(DescriptionKey, string.Empty);
            PriorityEntry.Text = Preferences.Get(PriorityKey, string.Empty);
            DueDateEntry.Text = Preferences.Get(DueDateKey, string.Empty);
        }

        private void OnSaveDataClicked(object sender, EventArgs e)
        {
            // Get the text from the Entry fields
            string task = TaskEntry.Text;
            string description = DescriptionEntry.Text;
            string priority = PriorityEntry.Text;
            string duedate = DueDateEntry.Text;

            // Save the data using Preferences API
            Preferences.Set(TaskKey, task);
            Preferences.Set(DescriptionKey, description);
            Preferences.Set(PriorityKey, priority);
            Preferences.Set(DueDateKey, duedate);

            // Display the saved data in the ResultLabel
            ResultLabel.Text = $"Saved Data: \nTask: {task} \nDescription: {description} \nPriority: {priority} \nDueDate: {duedate}";

            // Optionally, clear the entry fields
            TaskEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;
            DueDateEntry.Text = string.Empty;
        }
        private async void OnEditDataClicked(object sender, EventArgs e)
        {
            // Get the updated text from the Entry fields
            string updatedTask = TaskEntry.Text;
            string updatedDescription = DescriptionEntry.Text;
            string updatedPriority = PriorityEntry.Text;
            string updatedDueDate = DueDateEntry.Text;
            bool isCompleted = IsCompletedCheckBox.IsChecked;

            // Create a UserTask object with the updated values
            var updatedUserTask = new UserTask
            {
                TaskID = TaskId,
                UserId = UserId,
                Title = TitleLabel.Text,
                CreatedTask = updatedTask,
                Description = updatedDescription,
                PriorityLevel = int.TryParse(updatedPriority, out int priority) ? priority : 0,
                DueDate = DateTime.TryParse(updatedDueDate, out DateTime dueDate) ? dueDate : DateTime.Now,
                IsCompleted = isCompleted,
                UpdatedDate = DateTime.Now // Set to current time when editing
            };

            // Call the service to update the task
            try
            {
                await _userTaskService.UpdateTaskAsync(updatedUserTask);
                await DisplayAlert("Success", "Task updated successfully.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating task: {ex.Message}");
                await DisplayAlert("Error", "Failed to update task.", "OK");
            }
        }
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Confirm deletion
            bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete your saved data?", "Yes", "No");
            if (confirm)
            {
                // Remove the saved data
                Preferences.Remove("UserTask");
                Preferences.Remove("UserDescription");
                Preferences.Remove("UserPriority");
                Preferences.Remove("UserDueDate");
                // Clear the input fields
                TaskEntry.Text = string.Empty;
                DescriptionEntry.Text = string.Empty;
                PriorityEntry.Text = string.Empty;
                DueDateEntry.Text = string.Empty;

                DisplayAlert("Deleted", "Your data has been deleted!", "OK");
            }
        }

    }
}