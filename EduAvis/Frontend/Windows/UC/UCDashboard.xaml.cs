using EduAvis.Backend.Model;
using EduAvis.MVVM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using EduAvis.Resource.Utiles;
using EduAvis.Frontend.Windows.Dialog;
using System.IO;
using System.Text.Json;

namespace EduAvis.Frontend.Windows.UC
{
    public partial class UCDashboard : UserControl, IRefreshable
    {
        private BdeduavisContext _context;
        private Teacher _teacher;
        private MVDashboard _mvDashboard;

        public UCDashboard(MVDashboard mvDashboard, Teacher teacher, BdeduavisContext context)
        {
            InitializeComponent();
            _context = context;
            _teacher = teacher;
            _mvDashboard = mvDashboard;
            DataContext = _mvDashboard;

            txtTeacherName.Text = $"Welcome back, {_teacher.FullName}";
            txtCurrentDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy", new CultureInfo("en-US"));
        }

        public async Task Refresh()
        {
            await _mvDashboard.Start();
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            ViewIncidents viewIncidents = new ViewIncidents(_mvDashboard);
            viewIncidents.ShowDialog();
        }

        private void CmbTimeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MVDashboard viewModel)
            {
                var selectedIndex = ((ComboBox)sender).SelectedIndex;
                viewModel.UpdateChartData(selectedIndex);
            }
        }


        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var description = txtNewTask.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(description))
                {
                    _mvDashboard.AddTask(description);
                    txtNewTask.Text = string.Empty; 
                }
                else
                {
                    MessageBox.Show("Please enter a task description.", "Empty Task", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is UserTask task)
                {
                    _mvDashboard.RemoveTask(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void TxtNewTask_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AddTask_Click(sender, new RoutedEventArgs());
            }
        }
    }
}