using EduAvis.Backend.Model;
using EduAvis.Frontend.Windows;
using EduAvis.Frontend.Windows.UC;
using EduAvis.MVVM;
using EduAvis.Resource.Utiles;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace EduAvis
{
    public partial class MainWindow : Window
    {
        private bool isMenuExpanded = false;

        /// <summary>
        /// Context from database
        /// </summary>
        private BdeduavisContext _context;
        /// <summary>
        /// Teacher is the user that is logged in
        /// </summary>
        private Teacher _teacher;

        /// <summary>
        /// MVVM for incidents 
        /// </summary>
        private MVIncident mvIncident;
        private MVDashboard mvDashboard;
        private MVAdministration mvAdministration;

        public MainWindow(BdeduavisContext context, Teacher teacher)
        {
            InitializeComponent();
            _context = context;
            _teacher = teacher;

            // Need to this becasue Dashboard wasn't loading correctly
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            mvIncident = new MVIncident(_context, _teacher);
            await mvIncident.Start();

            mvDashboard = new MVDashboard(_context, _teacher);
            await mvDashboard.Start();

            mvAdministration = new MVAdministration(_context, _teacher);
            await mvAdministration.Start();

            btnDashboard_Click(this, new RoutedEventArgs());

            if (SessionManager.HasPermission(PermissionCodes.ManageRoles))
            {
                btnAdministration.Visibility = Visibility.Visible;
            }

        }



        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

        }
        

        private async void btnDashboard_Click(object sender, RoutedEventArgs e)
        { 
            UCDashboard ucDashboard = new UCDashboard(mvDashboard, _teacher, _context);
            await NavigateToAsync(ucDashboard);
        }


        private async void btnIncident_Click(object sender, RoutedEventArgs e)
        {
  
            UCIncidents ucIncidents = new UCIncidents(mvIncident, _teacher,_context);
            await NavigateToAsync(ucIncidents);
          
        }


        private async Task NavigateToAsync(UserControl uc)
        {
         
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uc);

            if (uc is IRefreshable refreshable)
            {
                await refreshable.Refresh();
            }
        }

        private void Logout()
        {
            // Clear the session data
            SessionManager.CurrentUser = null;
            SessionManager.Roles.Clear();
            SecureStorage.ClearCredentials();


            // Close the current window and open the login window
            Login loginWindow = new Login(_context);
            loginWindow.Show();

            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private async void btnAdministration_Click(object sender, RoutedEventArgs e)
        {
            UCAdministration ucAdministration = new UCAdministration(mvAdministration, _teacher,_context);
            mainPanel.Children.Clear();
            await NavigateToAsync(ucAdministration);
        }

        private async void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            UCSettings uc = new UCSettings(_teacher, _context,mvAdministration);
            await NavigateToAsync(uc);
        }
    }
}