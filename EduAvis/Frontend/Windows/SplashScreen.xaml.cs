using System;
using System.Threading.Tasks;
using System.Windows;
using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Frontend.Windows
{
    /// <summary>
    /// Lógica de interacción para SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private BdeduavisContext _context;

        public SplashScreen()
        {
            InitializeComponent();
            Loaded += SplashScreen_Loaded;
        }

        private async void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(3000); 

            bool dbConnected = await CheckDatabaseConnectionAsync();
            // Check if the database connection was successful
            if (dbConnected)
            {
                var savedCredentials = SecureStorage.LoadCredentials();

                // Check if there are saved credentials
                if (savedCredentials != null)
                {
                    string email = savedCredentials.Value.email;
                    string password = savedCredentials.Value.password;

                    var teacher = _context.Teachers.FirstOrDefault(t => t.Email == email);

                    if (teacher != null && teacher.Password == password)
                    {
                       
                        MainWindow mainWindow = new MainWindow(_context, teacher);
                        mainWindow.Show();
                        this.Close();
                        return;
                    }
                    else
                    {
                        //If aren't valid, clear the credentials
                        SecureStorage.ClearCredentials();
                    }
                }

              
                Login login = new Login(_context);
                login.Show();
                this.Close();
            }
        }

        private async Task<bool> CheckDatabaseConnectionAsync()
        {
            _context = new BdeduavisContext();
            try
            {
                await _context.Database.OpenConnectionAsync(); 
                _context.Database.CloseConnection();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Oops! We couldn't connect to the database. Please contact the administrator.",
                                "DATABASE CONNECTION",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                this.Close();
                return false;
            }
        }
    }
}
