using di.proyecto.clase._2024.MVVM.Base;
using EduAvis.Backend.Model;
using EduAvis.Resource.Utiles;
using EduAvis.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace EduAvis.MVVM
{
    public partial class MVIncident : MVBaseCRUD<MVIncident>
    {
        private BdeduavisContext _context;
        private IncidentService _incidentService;
        private StudentService _studentService;
        private GroupService _groupService;
        private ReasonService _reasonService;
        private TeacherService _teacherService;




        private string _filterStatus = "All";
        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(nameof(FilterStatus));
                OnPropertyChanged(nameof(FilteredIncidentsList));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(FilteredIncidentsList));
            }
        }
        private Group _filterGroup;
        public Group FilterGroup
        {
            get => _filterGroup;
            set
            {
                _filterGroup = value;
                OnPropertyChanged(nameof(FilterGroup));
                OnPropertyChanged(nameof(FilteredIncidentsList));
            }
        }


        public IEnumerable<Incident> FilteredIncidentsList
        {
            get
            {
                IEnumerable<Incident> list = incidentsList ?? Enumerable.Empty<Incident>();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    list = list.Where(i =>
                        $"{i.StudentNiaNavigation?.FirstName} {i.StudentNiaNavigation?.LastName}"
                        .ToLower().Contains(SearchText.ToLower()));
                }

                if (FilterGroup != null)
                {
                    list = list.Where(i => i.StudentNiaNavigation?.GroupCode == FilterGroup.GroupCode);
                }

                if (FilterStatus == "Sanctioned")
                {
                    list = list.Where(i => i.IsSanctioned);
                }
                else if (FilterStatus == "Pending")
                {
                    list = list.Where(i => !i.IsSanctioned);
                }

                return list;
            }
        }



        private Teacher _teacher;
        public Teacher teacher
        {
            get => _teacher;
            set { _teacher = value; OnPropertyChanged(nameof(teacher)); }
        }

        private Incident _incident;
        public Incident incident
            
        {
            get => _incident;
            set { _incident = value; OnPropertyChanged(nameof(incident)); }
        }

        private IEnumerable<Incident> _incidentsList;
        public IEnumerable<Incident> incidentsList
        {
            get => _incidentsList;
            set { _incidentsList = value; OnPropertyChanged(nameof(incidentsList)); }
        }

        private IEnumerable<Group> _groupList;
        public IEnumerable<Group> groupList
        {
            get => _groupList;
            set { _groupList = value; OnPropertyChanged(nameof(groupList)); }
        }

        private Group _groupSelected;
        public Group groupSelected
        {
            get => _groupSelected;
            set
            {
                _groupSelected = value;
                OnPropertyChanged(nameof(groupSelected));
                OnPropertyChanged(nameof(FilteredStudents));


            }
        }

        private Group _groupTeacherSelected;
        public Group groupTeacherSelected
        {
            get => _groupTeacherSelected;
            set
            {
                _groupTeacherSelected = value;
                OnPropertyChanged(nameof(groupTeacherSelected));
                OnPropertyChanged(nameof(FilteredTeachers));

            }
        }
        public IEnumerable<Student> FilteredStudents
        {
            get
            {
                if (groupSelected == null) return Enumerable.Empty<Student>();
                return studentsList?.Where(s => s.GroupCode == groupSelected.GroupCode) ?? Enumerable.Empty<Student>();
            }
        }
        public IEnumerable<Teacher> FilteredTeachers
        {
            get
            {
                if (groupTeacherSelected == null) return Enumerable.Empty<Teacher>();
                return teacherList?.Where(t => t.TutorGroup == groupTeacherSelected.GroupCode) ?? Enumerable.Empty<Teacher>();
            }
        }

        private IEnumerable<Student> _studentsList;
        public IEnumerable<Student> studentsList
        {
            get => _studentsList;
            set { _studentsList = value; OnPropertyChanged(nameof(studentsList)); }
        }

        private IEnumerable<Reason> _reasonList;
        public IEnumerable<Reason> reasonList
        {
            get => _reasonList;
            set { _reasonList = value; OnPropertyChanged(nameof(reasonList)); }
        }

        private IEnumerable<Teacher> _teacherList;
        public IEnumerable<Teacher> teacherList
        {
            get => _teacherList;
            set { _teacherList = value; OnPropertyChanged(nameof(teacherList)); }
        }

        public MVIncident(BdeduavisContext context,Teacher teacher)
        {
            _context = context;
            _teacher = teacher;

            date = DateTime.Today;
            time = DateTime.Today.Add(DateTime.Now.TimeOfDay);


        }


        // Nueva propiedad para la fecha
        private DateTime _date;
        public DateTime date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(date));
         
            }
        }

        // Nueva propiedad para la hora
        private DateTime _time;
        public DateTime time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(time));
   
            }
        }

        public async Task Start()
        {
            _incidentService = new IncidentService(_context);
            _studentService = new StudentService(_context);
            _groupService = new GroupService(_context);
            _reasonService = new ReasonService(_context);
            _teacherService = new TeacherService(_context);





            await FilterIncidentsByPermissions();

            groupList = await _groupService.GetAllAsync();
            reasonList = await _reasonService.GetAllAsync();
            teacherList = await _teacherService.GetAllAsync();
            studentsList = await _studentService.GetAllAsync();

        }


    
        
        public async Task LoadTeacherList()
        {
            teacherList = await _teacherService.GetAllAsync();
        }

        public async Task LoadStudentsByGroup()
        {
            if (groupSelected != null)
            {
                studentsList = await _studentService.FindStudentsByGroupAsync(groupSelected.GroupName);
            }
        }

        public async Task LoadTeacherByGroup()
        {
            if (groupTeacherSelected != null)
            {
                teacherList = await _teacherService.FindTeacherByGroupAsync(groupTeacherSelected.GroupName);
            }
        }

        

        public async Task<bool> saveIncident()
        {
            try
            {
                // Set the current date and time
                incident.RegisteredDatetime = DateTime.Now;

                // Set the teacher who is registering the incident
                incident.RegisteredBy = _teacher.Dni;
                incident.RegisteredByNavigation = _teacher;


                // If all required properties are set, save the incident
                bool correct = await _incidentService.AddAsync(incident);

                // Create a new incident for the next entry if successful
                if (correct)
                {
                    await FilterIncidentsByPermissions();
                    incident = new Incident();  
                    OnPropertyChanged(nameof(incident));

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

        public async Task<bool> updateIncident()
        {
            try
            {
              
                // If all required properties are set, save the incident
                bool correct = await _incidentService.UpdateAsync(incident);
                // Create a new incident for the next entry if successful
                if (correct)
                {
                    await FilterIncidentsByPermissions();
                    incident = new Incident();
                    OnPropertyChanged(nameof(incident));
                }
                return correct;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error saving incident: {ex.Message}");
                return false;
            }
        }


        public async Task LoadCBforModifyIncident()
        {

            groupTeacherSelected = await _teacherService.GetGroupByTeacherAsync(incident.TeacherDniNavigation);
            groupSelected = await _studentService.FindGroupByStudentAsync(incident.StudentNiaNavigation);


        }

        public async Task FilterIncidentsByPermissions()
        {
            var all = await _incidentService.GetAllAsync();
            string dni = _teacher.Dni;
            string? tutorGroup = _teacher.TutorGroup;

            if (SessionManager.HasPermission(PermissionCodes.ModifyOtherIncidents))
            {
                incidentsList = new ObservableCollection<Incident>(all);
                OnPropertyChanged(nameof(FilteredIncidentsList));
                return;
            }

            var filtered = all.Where(i =>
                (SessionManager.HasPermission(PermissionCodes.ModifyRegisteredIncidents) && i.RegisteredBy == dni) ||
                (SessionManager.HasPermission(PermissionCodes.ModifyOwnIncidents) && i.TeacherDni == dni) ||
                (SessionManager.HasPermission(PermissionCodes.ViewTutorGroupIncidents) &&
                 tutorGroup != null && i.StudentNiaNavigation?.GroupCode == tutorGroup)
            );

            incidentsList = new ObservableCollection<Incident>(filtered);
            OnPropertyChanged(nameof(FilteredIncidentsList));

        }

        public void UpdateIncidentInList(Incident updatedIncident)
        {
            if (incidentsList is ObservableCollection<Incident> observableList)
            {
                var existingIncident = observableList.FirstOrDefault(i => i.Id == updatedIncident.Id);
                if (existingIncident != null)
                {
                    // Actualiza las propiedades necesarias
                    existingIncident.IsSanctioned = updatedIncident.IsSanctioned;
                    // Actualiza otras propiedades si es necesario
                }
            }

            // Notifica el cambio en la lista filtrada
            OnPropertyChanged(nameof(FilteredIncidentsList));
        }

    }
}
