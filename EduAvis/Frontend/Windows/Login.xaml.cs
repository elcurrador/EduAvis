using EduAvis.Backend.Model;
using EduAvis.Resource.Utiles;
using Microsoft.EntityFrameworkCore;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;


namespace EduAvis.Frontend.Windows
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private BdeduavisContext _context;
        public Login(BdeduavisContext context)
        {
            InitializeComponent();
            _context = context;
          

        }
      


        private void buttonShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxPassword.Visibility == Visibility.Visible)
            {
                passwordBoxPassword.Visibility = Visibility.Collapsed;
                textBoxPassword.Visibility = Visibility.Visible;
                textBoxPassword.Text = passwordBoxPassword.Password;
                passwordVisibilityImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/eye_open.png"));

            }
            else
            {
                passwordBoxPassword.Visibility = Visibility.Visible;
                textBoxPassword.Visibility = Visibility.Collapsed;
                passwordBoxPassword.Password = textBoxPassword.Text;
                passwordVisibilityImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/eye_close.png"));
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = passwordBoxPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Incluir Role y sus RolePermissions y Permission en la misma consulta
            var teacher = _context.Teachers
                .Include(t => t.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(t => t.Email == email);

            if (teacher == null || teacher.Password != password)
            {
                MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (chkRememberMe.IsChecked == true)
                SecureStorage.SaveCredentials(email, password);
            else
                SecureStorage.ClearCredentials();

       
            SessionManager.CurrentUser = teacher;

           
            SessionManager.Roles = teacher.Role.RolePermissions
                .Select(rp => rp.Permission.Code)
                .ToList();

        


            MainWindow mainWindow = new MainWindow(_context, teacher);
            mainWindow.Show();
            this.Close();
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

    }


}
