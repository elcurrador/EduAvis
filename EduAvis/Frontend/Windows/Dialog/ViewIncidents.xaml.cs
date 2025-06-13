using EduAvis.MVVM;
using System.Windows;

namespace EduAvis.Frontend.Windows.Dialog
{
    public partial class ViewIncidents : Window
    {
        public ViewIncidents(MVDashboard mvDashboard)
        {
            InitializeComponent();
            DataContext = mvDashboard;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
