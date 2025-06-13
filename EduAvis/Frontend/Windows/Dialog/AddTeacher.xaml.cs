using EduAvis.Backend.Model;
using EduAvis.MVVM;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EduAvis.Frontend.Windows.Dialog
{
    /// <summary>
    /// Lógica de interacción para AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window
    {
        private MVAdministration _mvAdministration;

        public AddTeacher(MVAdministration mVAdministration)
        {
            InitializeComponent();
            _mvAdministration = mVAdministration;
            DataContext = _mvAdministration;
            _mvAdministration.NewTeacher = new Teacher();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxPassword.Visibility == Visibility.Visible)
            {
                // Cambiar a TextBox visible
                passwordBoxPassword.Visibility = Visibility.Collapsed;
                textBoxPassword.Visibility = Visibility.Visible;
                textBoxPassword.Text = passwordBoxPassword.Password;
                passwordVisibilityImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/eye_open.png"));
            }
            else
            {
                // Cambiar a PasswordBox visible
                passwordBoxPassword.Visibility = Visibility.Visible;
                textBoxPassword.Visibility = Visibility.Collapsed;
                passwordBoxPassword.Password = textBoxPassword.Text;
                passwordVisibilityImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/eye_close.png"));
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mvAdministration?.NewTeacher != null)
                {
         
                    UpdatePasswordInModel();

                    var validationErrors = _mvAdministration.NewTeacher.GetValidationErrors();
                    if (validationErrors.Any())
                    {
                        string errorMessage = string.Join("\n", validationErrors);
                        MessageBox.Show($"Validation errors:\n{errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    bool success = await _mvAdministration.saveTeacher();

                    if (success)
                    {
                        MessageBox.Show("Teacher saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error saving teacher.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                string errorDetails = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Database error:\n{errorDetails}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void UpdatePasswordInModel()
        {
            if (_mvAdministration?.NewTeacher != null)
            {
                _mvAdministration.NewTeacher.Password = passwordBoxPassword.Visibility == Visibility.Visible
                    ? passwordBoxPassword.Password
                    : textBoxPassword.Text;
            }
        }

        private void passwordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordInModel();
        }


        private void textBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePasswordInModel();
        }

        private void cmbTutorGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}