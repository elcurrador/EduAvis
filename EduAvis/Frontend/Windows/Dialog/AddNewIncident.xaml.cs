using EduAvis.Backend.Model;
using EduAvis.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para AddNewIncident.xaml
    /// </summary>
    public partial class AddNewIncident : Window
    {

        private MVIncident _mvIncident;
        private Teacher _teacher;
        private Incident auxIncident;

        private List<Teacher> _allTeachers;
        private List<Group> _allGroups;

        public AddNewIncident(MVIncident mvIncident, Teacher teacher)
        {
            InitializeComponent();
           
            _mvIncident = mvIncident;
            _teacher = teacher;
            DataContext = _mvIncident;

    
        
        }




        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = eventDatePicker.SelectedDate;
            DateTime? selectedTime = eventTimePicker.SelectedTime;

            if (selectedDate.HasValue && selectedTime.HasValue)
            {
                // Convertir correctamente
                DateTime eventDateTime = selectedDate.Value.Date + selectedTime.Value.TimeOfDay;

                // Usar el valor combinado
                _mvIncident.incident.EventDatetime = eventDateTime;
            }




            try
            {
                if (_mvIncident?.incident != null)
                {
                    bool success = await _mvIncident.saveIncident();

             


                    if (success)
                    {
                        MessageBox.Show("The incident was saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("The incident could not be saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

           
        


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void cbIsWritingReport_Unchecked(object sender, RoutedEventArgs e)
        {
            cbGroupTeacher.IsEnabled = true;
            cbTeacher.IsEnabled = true;

            _mvIncident.LoadTeacherList();



            _mvIncident.groupTeacherSelected = null;
        }


        private void cbIsWritingReport_Checked(object sender, RoutedEventArgs e)
        {
            cbGroupTeacher.IsEnabled = false;
            cbTeacher.IsEnabled = false;



                _mvIncident.incident.TeacherDni = _teacher.Dni;
              
            }
        }




    
}
