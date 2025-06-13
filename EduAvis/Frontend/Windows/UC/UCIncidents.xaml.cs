using EduAvis.Backend.Model;
using EduAvis.Frontend.Windows.Dialog;
using EduAvis.MVVM;
using EduAvis.Resource.Utiles;
using EduAvis.Service;
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
    /// Lógica de interacción para UCIncidents.xaml
    /// </summary>
    public partial class UCIncidents : UserControl, IRefreshable
    {

        private MVIncident _mvIncident;

        private Teacher _teacher;

        private BdeduavisContext _conext;

        IncidentService _incidentService;

        public UCIncidents(MVIncident mvIncident,Teacher teacher,BdeduavisContext context)
        {
            InitializeComponent();
            _mvIncident = mvIncident;
            _teacher = teacher;
            DataContext = _mvIncident;
            _conext = context;

            _incidentService = new IncidentService(_conext);
            

        }

        private async void btnAddIncident_Click(object sender, RoutedEventArgs e)
        {
       
            _mvIncident.incident = new Incident();
            _mvIncident.groupTeacherSelected = null;
            _mvIncident.groupSelected = null;
            AddNewIncident addNewIncident = new AddNewIncident(_mvIncident, _teacher);
            addNewIncident.ShowDialog();
        }


        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Incident incident)
            {
                _mvIncident.incident = incident;
                _mvIncident.date = incident.EventDatetime.Date;
                _mvIncident.time = DateTime.Today.Add(incident.EventDatetime.TimeOfDay);
                await _mvIncident.LoadCBforModifyIncident(); 

                ModifyIncident modifyIncident = new ModifyIncident(_mvIncident, _teacher);
                modifyIncident.ShowDialog();


            }
            else
            {
                MessageBox.Show("ERROR 404");
            }
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Incident incident)
            {
                var result = MessageBox.Show("¿Are you sure you want delete this incident?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _incidentService.DeleteAsync(incident); 
                    await _mvIncident.FilterIncidentsByPermissions();  
                }
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }
        }

        private void clearFilters_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MVIncident vm)
            {
                vm.SearchText = string.Empty;
                vm.FilterGroup = null;
                vm.FilterStatus = "All";

             
            }
        }

        public async Task Refresh()
        {
            await _mvIncident.Start();
        }


        private async void btnInform_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement fe && fe.DataContext is Incident incident)
                {
                    _mvIncident.incident = incident;

                    EmailService emailService = new EmailService();
                    string body = emailService.BuildIncidentEmailBody(incident);
                    string toEmail = incident.StudentNiaNavigation?.Email;

                    if (string.IsNullOrWhiteSpace(toEmail))
                    {
                        MessageBox.Show("No email address found for the student.", "Missing Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool success = await emailService.SendEmailAsync(toEmail, body);

                    if (success)
                    {
                        MessageBox.Show("Incident report sent successfully.", "Email Sent", MessageBoxButton.OK, MessageBoxImage.Information);
                        _mvIncident.incident.IsSanctioned = true; // Mark the incident as sanctioned after sending the email
                        await _mvIncident.updateIncident();

                        _mvIncident.UpdateIncidentInList(incident);
                    }
                    else
                    {
                        MessageBox.Show("The email could not be sent. Please try again later.", "Sending Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Incident data not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred:\n{ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
