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
    /// Lógica de interacción para UCAdministration.xaml
    /// </summary>
    public partial class UCAdministration : UserControl
    {

        private MVAdministration _mvAdministration;
        private Teacher _teacher;
        private BdeduavisContext _context;


        private TeacherService _teacherService;
        private StudentService _studentService;

        public UCAdministration(MVAdministration mvAdministration, Teacher teacher,BdeduavisContext context)
        {
            InitializeComponent();
            _mvAdministration = mvAdministration;
            _teacher = teacher;
            DataContext = _mvAdministration;
            _context = context;

            _teacherService = new TeacherService(_context);
            _studentService = new StudentService(_context);

        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MVAdministration vm)
            {
                vm.SearchTeacherText = string.Empty;
                vm.SelectedRoleFilter = null;
            }

            
        }


        private void btnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            AddTeacher addTeacher = new AddTeacher(_mvAdministration);
            addTeacher.ShowDialog();
        }

        private void btnEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Teacher teacher)
            {
               _mvAdministration.Teacher = teacher;
                _mvAdministration.SelectedRole = _mvAdministration.RoleList?
         .FirstOrDefault(r => r.Id == teacher.RoleId);

                ModifyTeacher modifyTeacher = new ModifyTeacher(_mvAdministration);
                modifyTeacher.ShowDialog();
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }
        }
        private async void btnDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Teacher teacher)
            {
                var result = MessageBox.Show("¿Are you sure you want delete this teacher?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _teacherService.DeleteAsyncPro(teacher);
            
                    await _mvAdministration.RefreshTeachersAsync();
                }
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }
        }
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudent addStudent = new AddStudent(_mvAdministration);
            addStudent.ShowDialog();
        }
        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Student student)
            {
                _mvAdministration.Student = student;
                ModifyStudent modifyStudent = new ModifyStudent(_mvAdministration);
                modifyStudent.ShowDialog();
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }

        }
        private async void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Student student)
            {
                var result = MessageBox.Show("¿Are you sure you want delete this student?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _studentService.DeleteAsyncPro(student);
       
                    await _mvAdministration.RefreshStudentsAsync();
                }
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }
        }

        private void btnClearFiltersStudents_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MVAdministration vm)
            {
                vm.SearchStudentText = string.Empty;
                vm.SelectedGroupFilter = null;
            }
        }


        private void btnEditRole_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Role role)
            {
                _mvAdministration.Role = role;

                ModifyRole modifyRole = new ModifyRole(role, _mvAdministration);
                _mvAdministration.LoadOwnPermissionsAsync();
                modifyRole.ShowDialog();
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }

        }

        private void dgTeachers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
