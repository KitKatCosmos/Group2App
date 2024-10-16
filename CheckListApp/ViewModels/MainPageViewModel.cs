using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckListApp.Services;
using CheckListApp.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace CheckListApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly AuthenticationService _authenticationService;
        private readonly UserTaskService _userTaskService;

        private ObservableCollection<UserTask> _userTasks;
        private bool? _isLoading;
        private string? _errorMessage;

        public MainPageViewModel(AuthenticationService authenticationService, UserTaskService userTaskService)
        {
            _authenticationService = authenticationService;
            _userTaskService = userTaskService;
            _userTasks = new ObservableCollection<UserTask>();
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        public IAsyncRelayCommand LoginCommand { get; }

        public ObservableCollection<UserTask> UserTasks
        {
            get => _userTasks;
            set => SetProperty(ref _userTasks, value);
        }

        public bool IsLoading
        {
            get => (bool)_isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async Task LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Bypass authentication and directly fetch tasks for user ID 1
                var tasks = await _userTaskService.GetTasksForUserAsync(1);

                UserTasks.Clear();
                foreach (var task in tasks)
                {
                    UserTasks.Add(task);
                }

                // If you want to navigate to a new page after loading tasks, you can do it here
                // For example:
                // await Shell.Current.GoToAsync("//UserTasksPage");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading tasks: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}