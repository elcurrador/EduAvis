using di.proyecto.clase._2024.MVVM.Base;
using EduAvis.Backend.Model;
using EduAvis.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EduAvis.MVVM
{
    public partial class MVAdministration : MVBaseCRUD<Teacher>
    {
        private readonly BdeduavisContext _context;
        private readonly Teacher _currentUser;

        private TeacherService _teacherService;
        private StudentService _studentService;
        private RoleService _roleService;
        private GroupService _groupService;
        private PermissionsService _permissionsService;

        // Constructor
        public MVAdministration(BdeduavisContext context, Teacher currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // Initialization logic
        public async Task Start()
        {
            _teacherService = new TeacherService(_context);
            _studentService = new StudentService(_context);
            _roleService = new RoleService(_context);
            _groupService = new GroupService(_context);
            _permissionsService = new PermissionsService(_context);
            // Base CRUD service
            service = _teacherService;

            GroupList = await _groupService.GetAllAsync();
            RoleList = await _roleService.GetAllAsync();
            PermissionList = await _permissionsService.GetAllAsync();
            RoleListAdmin = RoleList.Where(r => r.Id != _currentUser.RoleId);
            _allPermissions = await _permissionsService.GetAllAsync();
            RoleFilterOptions = await _roleService.GetAllAsync();
            await LoadOwnPermissionsAsync();
            await RefreshTeachersAsync();
            await RefreshStudentsAsync();
        }

        public async Task LoadOwnPermissionsAsync()
        {
            if (Role != null)
            {
                OwnPermissions = await _permissionsService.GetPermissionsByRoleIdAsync(Role.Id);
            }
            else
            {
                OwnPermissions = new List<Permission>();
            }

  
            UpdateFilteredPermissionList();
        }


        private void UpdateFilteredPermissionList()
        {
            if (_allPermissions != null && OwnPermissions != null)
            {

                var ownPermissionIds = OwnPermissions.Select(p => p.Id).ToHashSet();
                PermissionList = _allPermissions.Where(p => !ownPermissionIds.Contains(p.Id));
            }
            else
            {
                PermissionList = _allPermissions ?? new List<Permission>();
            }
        }

        private ObservableCollection<Teacher> _teacherList;
        public ObservableCollection<Teacher> TeacherList
        {
            get => _teacherList;
            set
            {
                _teacherList = value;
                OnPropertyChanged(nameof(TeacherList));
                OnPropertyChanged(nameof(FilteredTeacherList));
            }
        }
        private IEnumerable<Permission> _allPermissions;
  
        private IEnumerable<Permission> _permissionList;
        public IEnumerable<Permission> PermissionList
        {
            get => _permissionList;
            set
            {
                _permissionList = value;
                OnPropertyChanged(nameof(PermissionList));
            }
        }
   

        private IEnumerable<Role> _roleFilterOptions;
        public IEnumerable<Role> RoleFilterOptions
        {
            get => _roleFilterOptions;
            set
            {
                _roleFilterOptions = value;
                OnPropertyChanged(nameof(RoleFilterOptions));
            }
        }
        private IEnumerable<Role> _roleListAdmin;
        public IEnumerable<Role> RoleListAdmin
        {
            get => _roleListAdmin;
            set
            {
                _roleListAdmin = value;
                OnPropertyChanged(nameof(RoleListAdmin));
            }
        }

        // Observable student list
        private ObservableCollection<Student> _studentList;
        public ObservableCollection<Student> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
                OnPropertyChanged(nameof(FilteredStudentList));
            }
        }
        private Student _student;
        public Student Student
        {
            get => _student;
            set { _student = value; OnPropertyChanged(nameof(Student)); }
        }

        private IEnumerable<Permission> _ownPermissions;
        public IEnumerable<Permission> OwnPermissions
        {
            get => _ownPermissions;
            set
            {
                _ownPermissions = value;
                OnPropertyChanged(nameof(OwnPermissions));
            }
        }



        private Teacher _newTeacher;
        public Teacher NewTeacher
        {
            get => _newTeacher;
            set
            {
                _newTeacher = value;
                OnPropertyChanged(nameof(NewTeacher));
            }
        }

        private Teacher _teacher;
        public Teacher Teacher
        {
            get => _teacher;
            set
            {
                _teacher = value;
                OnPropertyChanged(nameof(Teacher));
            }
        }
        private Role _role;
        public Role Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        // Available roles for teacher assignment
        private IEnumerable<Role> _roleList;
        public IEnumerable<Role> RoleList
        {
            get => _roleList;
            set
            {
                _roleList = value;
                OnPropertyChanged(nameof(RoleList));
            }
        }

        private Permission _selectedAvailablePermission;
        public Permission SelectedAvailablePermission
        {
            get => _selectedAvailablePermission;
            set { _selectedAvailablePermission = value; OnPropertyChanged(nameof(SelectedAvailablePermission)); }
        }

        private Permission _selectedOwnPermission;
        public Permission SelectedOwnPermission
        {
            get => _selectedOwnPermission;
            set { _selectedOwnPermission = value; OnPropertyChanged(nameof(SelectedOwnPermission)); }
        }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));

                    if (NewTeacher != null && _selectedRole != null)
                    {
                        NewTeacher.RoleId = _selectedRole.Id;
                    }
                    if (Teacher != null && _selectedRole != null)
                    {
                        Teacher.RoleId = _selectedRole.Id;
                    }

                    OnPropertyChanged(nameof(IsTutorRoleSelected));
                }
            }
        }


        private IEnumerable<Group> _groupList;
        public IEnumerable<Group> GroupList
        {
            get => _groupList;
            set
            {
                _groupList = value;
                OnPropertyChanged(nameof(GroupList));
            }
        }



        public bool IsTutorRoleSelected =>
    SelectedRole?.Name?.ToLower().Contains("tutor") == true;







        public async Task RefreshTeachersAsync()
        {
            var list = await _teacherService.getAllAsycAndRoles();
            TeacherList = new ObservableCollection<Teacher>(list);
        }

        public async Task RefreshStudentsAsync()
        {
            var list = await _studentService.GetAllAsync();
            StudentList = new ObservableCollection<Student>(list);
        }


        public async Task<bool> saveTeacher()
        {
            try
            {

                bool dniExists = await _teacherService.DniExistsAsync(NewTeacher.Dni);
                if (dniExists)
                {
                    MessageBox.Show("A teacher with this DNI already exists.", "Duplicate DNI", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                bool emailExists = await _teacherService.EmailExistsAsync(NewTeacher.Email);
                if (emailExists)
                {
                    MessageBox.Show("A teacher with this email already exists.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }


                if (IsTutorRoleSelected && NewTeacher.IsTutor)
                {
                    bool hasMainTutor = await _teacherService.HasMainTutorInGroupAsync(NewTeacher.TutorGroup);

                    if (hasMainTutor)
                    {
                        MessageBox.Show("This group already has a main tutor. Please assign as co-tutor or choose another group.",
                                        "Main Tutor Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                bool correct = await _teacherService.AddAsync(NewTeacher);

                // Create a new incident for the next entry if successful
                if (correct)
                {
                    NewTeacher = new Teacher();
                   await Start(); // Reinitialize the view model
                    SelectedRole = null;
                    OnPropertyChanged(nameof(IsTutorRoleSelected));


                }

                return correct;
            }
            catch (Exception ex)
            {
                // Log error or handle exception
                Console.WriteLine($"Error saving incident: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> updateTeacher()
        {
            try
            {

                bool emailExists = await _teacherService.EmailExistsAsync(Teacher.Email, Teacher.Dni);
                if (emailExists)
                {
                    MessageBox.Show("A teacher with this email already exists.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (IsTutorRoleSelected && Teacher.IsTutor)
                {
                    bool hasMainTutor = await _teacherService.HasMainTutorInGroupAsync(Teacher.TutorGroup);

                    if (hasMainTutor)
                    {
                        MessageBox.Show("This group already has a main tutor. Please assign as co-tutor or choose another group.",
                                        "Main Tutor Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                bool correct = await _teacherService.UpdateAsync(Teacher);

                if (correct)
                {
                    NewTeacher = new Teacher();
                    await Start(); // Reinitialize the view model
                    SelectedRole = null;
                    OnPropertyChanged(nameof(IsTutorRoleSelected));


                }

                return correct;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving incident: {ex.Message}");
                return false;

            }
    }
        public async Task<bool> saveStudent()
        {
            try
            {
                bool niaExists = await _studentService.NiaExistsAsync(Student.Nia);
                if (niaExists)
                {
                    MessageBox.Show("A student with this NIA already exists.", "Duplicate NIA", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }


                bool correct = await _studentService.AddAsync(Student);

                if (correct)
                {
                    Student = new Student();
                    await Start(); // Reinitialize the view model
                    SelectedRole = null;
                


                }

                return correct;
            }
            catch (Exception ex)
            {
                // Log error or handle exception
                Console.WriteLine($"Error saving incident: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> updateStudent()
        {
            try
            {
                bool correct = await _studentService.UpdateAsync(Student);
                if (correct)
                {
                    Student = new Student();
                    await Start(); // Reinitialize the view model
                  
                }
                return correct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> updateTeacherPassword(Teacher teacher)
        {
            try
            {
                bool correct = await _teacherService.UpdateAsync(teacher);

                if (correct)
                {
                    NewTeacher = new Teacher();
                    await Start(); 
                 
                }

                return correct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving incident: {ex.Message}");
                return false;

            }
        }

        public async Task<bool> SaveRolePermissionsAsync()
        {
            try
            {
                if (Role == null || OwnPermissions == null)
                    return false;

                await _permissionsService.UpdateRolePermissionsAsync(Role.Id, OwnPermissions.ToList());
                MessageBox.Show("Permissions updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating permissions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        //Filters

        private string _searchTeacherText;
        public string SearchTeacherText
        {
            get => _searchTeacherText;
            set
            {
                _searchTeacherText = value;
                OnPropertyChanged(nameof(SearchTeacherText));
                OnPropertyChanged(nameof(FilteredTeacherList));
            }
        }
        private Role _selectedRoleFilter;
        public Role SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            set { _selectedRoleFilter = value; OnPropertyChanged(nameof(SelectedRoleFilter)); OnPropertyChanged(nameof(FilteredTeacherList)); }
        }
        public IEnumerable<Teacher> FilteredTeacherList =>
      TeacherList
          .Where(t =>
              (string.IsNullOrWhiteSpace(SearchTeacherText) ||
               (!string.IsNullOrWhiteSpace(t.FirstName) && t.FirstName.Contains(SearchTeacherText, StringComparison.OrdinalIgnoreCase)) ||
               (!string.IsNullOrWhiteSpace(t.LastName) && t.LastName.Contains(SearchTeacherText, StringComparison.OrdinalIgnoreCase)))
              &&
               (SelectedRoleFilter == null || t.Role?.Name == SelectedRoleFilter.Name)
          );

        private string _searchStudentText;
        public string SearchStudentText
        {
            get => _searchStudentText;
            set
            {
                _searchStudentText = value;
                OnPropertyChanged(nameof(SearchStudentText));
                OnPropertyChanged(nameof(FilteredStudentList));
            }
        }

        private Group _selectedGroupFilter;
        public Group SelectedGroupFilter
        {
            get => _selectedGroupFilter;
            set
            {
                _selectedGroupFilter = value;
                OnPropertyChanged(nameof(SelectedGroupFilter));
                OnPropertyChanged(nameof(FilteredStudentList));
            }
        }

        public IEnumerable<Student> FilteredStudentList =>
            StudentList
                .Where(s =>
                    (string.IsNullOrWhiteSpace(SearchStudentText) ||
                     (!string.IsNullOrWhiteSpace(s.FirstName) && s.FirstName.Contains(SearchStudentText, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrWhiteSpace(s.LastName) && s.LastName.Contains(SearchStudentText, StringComparison.OrdinalIgnoreCase)))
                    &&
                    (SelectedGroupFilter == null || s.GroupCode == SelectedGroupFilter.GroupCode)
                );

    }
}
