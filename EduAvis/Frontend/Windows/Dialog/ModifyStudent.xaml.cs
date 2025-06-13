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
    /// Lógica de interacción para AddStudent.xaml
    /// </summary>
    public partial class ModifyStudent : Window
    {

        private MVAdministration _mvAdministration;

        public ModifyStudent(MVAdministration mvAdministration)
        {
            InitializeComponent();
            _mvAdministration = mvAdministration;
            DataContext = _mvAdministration;
       
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mvAdministration?.Student != null)
                {


                    var validationErrors = _mvAdministration.Student.GetValidationErrors();
                    if (validationErrors.Any())
                    {
                        string errorMessage = string.Join("\n", validationErrors);
                        MessageBox.Show($"Validation errors:\n{errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                   
                    bool success = await _mvAdministration.updateStudent();

                    if (success)
                    {
                        MessageBox.Show("Student updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error updating student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
