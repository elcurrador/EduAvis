using EduAvis.Backend.Model;
using EduAvis.MVVM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EduAvis.Frontend.Windows.UC
{
    /// <summary>
    /// Lógica de interacción para UCSettings.xaml
    /// </summary>
    public partial class UCSettings : UserControl
    {

        private Teacher _teacher;
        private BdeduavisContext _context;
        private MVAdministration _mvAdministration;
        public UCSettings(Teacher teacher, BdeduavisContext context,MVAdministration mvAdministration)
        {
            InitializeComponent();
            _teacher = teacher;
            _context = context;
            _mvAdministration = mvAdministration;
            DataContext = _mvAdministration;


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_teacher.Password == CurrentPasswordBox.Password)
            {
                if (NewPasswordBox.Password == RepeatPasswordBox.Password)
                {
                    _teacher.Password = NewPasswordBox.Password;
                    await _mvAdministration.updateTeacherPassword(_teacher);
                    MessageBox.Show("Password updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("New passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CurrentPasswordBox.Clear();
            NewPasswordBox.Clear();
            RepeatPasswordBox.Clear();



        }
    }
}