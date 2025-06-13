using di.proyecto.clase._2024.MVVM.Base;
using EduAvis.Backend.Model;
using EduAvis.Resource.Utiles;
using EduAvis.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EduAvis.MVVM
{
    public partial class MVDashboard : MVBaseCRUD<MVDashboard>
    {
        private BdeduavisContext _context;
        private Teacher _teacher;
        private IncidentService _incidentService;

        public SeriesCollection ChartSeries { get; set; }
        public ObservableCollection<string> DaysLabels { get; set; }
        public ObservableCollection<Incident> FilteredIncidents { get; set; } = new();

  

        public MVDashboard(BdeduavisContext context, Teacher teacher)
        {
            _context = context;
            _teacher = teacher;
            ChartSeries = new SeriesCollection();
            DaysLabels = new ObservableCollection<string>();
            CurrentDni = teacher.Dni;

            UserTasks = new ObservableCollection<UserTask>();
            LoadTasks();
            UserTasks.CollectionChanged += (s, e) => SaveTasks();
        }

        public async Task Start()
        {
            _incidentService = new IncidentService(_context);
            await FilterIncidentsByPermissions();
            SeparateIncidents();
            InitializeChart();
        }

        

        private void SeparateIncidents()
        {
            if (incidentsList == null)
            {
                totalWarnings = "0";
                totalSanctions = "0";
                totalIncidents = "0";
                return;
            }

            totalWarnings = incidentsList.Count(i => !i.isSanction).ToString();
            totalSanctions = incidentsList.Count(i => i.isSanction).ToString();
            totalIncidents = incidentsList.Count().ToString();
        }

        private IEnumerable<Incident> _incidentsList;
        public IEnumerable<Incident> incidentsList
        {
            get => _incidentsList;
            set
            {
                _incidentsList = value;
                OnPropertyChanged(nameof(incidentsList));
                OnPropertyChanged(nameof(Last5Incidents));
            }
        }

        public IEnumerable<Incident> Last5Incidents =>
    incidentsList?
        .OrderByDescending(i => i.EventDatetime)
        .ThenByDescending(i => i.Id) 
        .Take(5) ??
    Enumerable.Empty<Incident>();


        private string _totalWarnings;
        public string totalWarnings
        {
            get => _totalWarnings;
            set { _totalWarnings = value; OnPropertyChanged(nameof(totalWarnings)); }
        }

        private string _totalSanctions;
        public string totalSanctions
        {
            get => _totalSanctions;
            set { _totalSanctions = value; OnPropertyChanged(nameof(totalSanctions)); }
        }

        private string _totalIncidents;
        public string totalIncidents
        {
            get => _totalIncidents;
            set { _totalIncidents = value; OnPropertyChanged(nameof(totalIncidents)); }
        }

        public async Task FilterIncidentsByPermissions()
        {
            var all = await _incidentService.GetAllAsync();
            string dni = _teacher.Dni;
            string? tutorGroup = _teacher.TutorGroup;

            if (SessionManager.HasPermission(PermissionCodes.ModifyOtherIncidents))
            {
                incidentsList = new ObservableCollection<Incident>(all);
                return;
            }

            var filtered = all.Where(i =>
                (SessionManager.HasPermission(PermissionCodes.ModifyRegisteredIncidents) && i.RegisteredBy == dni) ||
                (SessionManager.HasPermission(PermissionCodes.ModifyOwnIncidents) && i.TeacherDni == dni) ||
                (SessionManager.HasPermission(PermissionCodes.ViewTutorGroupIncidents) &&
                 tutorGroup != null && i.StudentNiaNavigation?.GroupCode == tutorGroup));

            incidentsList = new ObservableCollection<Incident>(filtered);
        }

        public string CurrentDni { get; set; }
        public ObservableCollection<UserTask> UserTasks { get; set; }

        public void SaveTasks()
        {
            try { TaskService.SaveTasks(CurrentDni, UserTasks.ToList()); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"SaveTasks error: {ex.Message}"); }
        }

        private void InitializeChart()
        {
            UpdateChartData(0);
        }

        public void UpdateChartData(int timeFilterIndex)
        {
            if (incidentsList == null) return;

            DateTime startDate = GetStartDateFromFilter(timeFilterIndex);
            var filteredIncidents = incidentsList.Where(i => i.EventDatetime >= startDate);

            // Agrupar por fecha
            var groupedData = filteredIncidents
                .GroupBy(i => i.EventDatetime.Date)
                .OrderBy(g => g.Key)
                .ToList();

            // Limpiar datos anteriores
            ChartSeries.Clear();
            DaysLabels.Clear();

            if (!groupedData.Any()) return;

            // Preparar datos para las series
            var sanctionValues = new ChartValues<int>();
            var warningValues = new ChartValues<int>();

            foreach (var group in groupedData)
            {
                DaysLabels.Add(group.Key.ToString("MM/dd"));
                sanctionValues.Add(group.Count(i => i.isSanction));
                warningValues.Add(group.Count(i => !i.isSanction));
            }

            // Crear las series
            ChartSeries.Add(new LineSeries
            {
                Title = "Sanctions",
                Values = sanctionValues,
                Stroke = Brushes.Red,
                Fill = Brushes.Transparent,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 8
            });

            ChartSeries.Add(new LineSeries
            {
                Title = "Warnings",
                Values = warningValues,
                Stroke = Brushes.Orange,
                Fill = Brushes.Transparent,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 8
            });

            OnPropertyChanged(nameof(ChartSeries));
            OnPropertyChanged(nameof(DaysLabels));
        }

        private DateTime GetStartDateFromFilter(int filterIndex)
        {
            return filterIndex switch
            {
                1 => DateTime.Now.AddDays(-7),   // 1 Week Ago
                2 => DateTime.Now.AddMonths(-1), // 1 Month Ago
                3 => DateTime.Now.AddYears(-1),  // 1 Year Ago
                _ => DateTime.MinValue           // All Time
            };
        }

        public void AddTask(string taskTitle)
        {
            if (!string.IsNullOrWhiteSpace(taskTitle))
            {
                UserTasks.Add(new UserTask
                {
                    Dni = CurrentDni,
                    Title = taskTitle.Trim(),
                    Description = taskTitle.Trim(),
                    IsCompleted = false
                });
            }
        }

        public void RemoveTask(UserTask task)
        {
            if (task != null && UserTasks.Contains(task)) UserTasks.Remove(task);
        }

        public void LoadTasks()
        {
            try
            {
                var tasks = TaskService.LoadTasks(CurrentDni);
                UserTasks.Clear();
                foreach (var task in tasks) UserTasks.Add(task);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadTasks error: {ex.Message}");
            }
        }
    }
}
