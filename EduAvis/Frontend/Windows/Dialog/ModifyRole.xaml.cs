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
using System.Windows.Shapes;

namespace EduAvis.Frontend.Windows.Dialog
{
    /// <summary>
    /// Lógica de interacción para ModifyRole.xaml
    /// </summary>
    public partial class ModifyRole : Window
    {

        private Role _role;
        private MVAdministration _mvAdministration;


        public ModifyRole(Role role, MVAdministration mvAdministration)
        {
            InitializeComponent();
            _role = role;
            _mvAdministration = mvAdministration;
            DataContext = _mvAdministration;
        }

        private void AddPermission_Click(object sender, RoutedEventArgs e)
        {
            var selected = _mvAdministration.SelectedAvailablePermission;
            if (selected != null)
            {
                var ownList = _mvAdministration.OwnPermissions.ToList();
                ownList.Add(selected);
                _mvAdministration.OwnPermissions = ownList;

                var available = _mvAdministration.PermissionList.ToList();
                available.Remove(selected);
                _mvAdministration.PermissionList = available;
            }
        }

        private void RemovePermission_Click(object sender, RoutedEventArgs e)
        {
            var selected = _mvAdministration.SelectedOwnPermission;
            if (selected != null)
            {
                var available = _mvAdministration.PermissionList.ToList();
                available.Add(selected);
                _mvAdministration.PermissionList = available;

                var ownList = _mvAdministration.OwnPermissions.ToList();
                ownList.Remove(selected);
                _mvAdministration.OwnPermissions = ownList;
            }
        }


        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool success = await _mvAdministration.SaveRolePermissionsAsync();
            if (success)
                this.Close();
        }

    }
}
