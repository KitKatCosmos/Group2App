using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckListApp.ViewModels;
using CheckListApp.Model;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CheckListApp.View
{
    public partial class UserTaskPage : ContentPage
    {
        private UserTaskViewModel _viewModel;

        public UserTaskPage(UserTaskViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadUserAndTasksCommand.Execute(null);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                var task = args.SelectedItem as UserTask;
                if (task != null)
                {
                    if (_viewModel.SelectTaskCommand?.CanExecute(task) == true)
                    {
                        await _viewModel.SelectTaskCommand.ExecuteAsync(task);
                    }
                    else
                    {
                        Debug.WriteLine($"Navigating to ItemDetailPage with TaskID: {task.TaskID} and UserID: {task.UserId}");
                        await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?id={task.TaskID}&userId={task.UserId}");
                    }
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}