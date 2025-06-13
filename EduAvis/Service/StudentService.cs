using System.Windows;
using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Service
{
    public class StudentService : GenericService<Student>
    {
        private BdeduavisContext _context;
        public StudentService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> FindStudentsByGroupAsync(string groupName)
        {
            try
            {
                // Filtrar los estudiantes por el nombre del grupo  
                var students = await _context.Students
                    .Where(s => s.GroupCodeNavigation.GroupName == groupName)
                    .ToListAsync();

                logger.Info($"Students found in group: {groupName}");
                return students;
            }
            catch (Exception ex)
            {
                // Cambiar ErrorLog a un método accesible o implementar un método local para manejar errores  
                LogError("Error while fetching students by group", ex);
                return Enumerable.Empty<Student>();
            }
        }
        public async Task<Group?> FindGroupByStudentAsync(Student student)
        {
            if (student == null || string.IsNullOrEmpty(student.GroupCode))
                return null;

            return await _context.Groups
                .FirstOrDefaultAsync(g => g.GroupCode == student.GroupCode);
        }

     
        private void LogError(string message, Exception ex)
        {
           
            logger.Error(message, ex);
        }

        public async Task<bool> NiaExistsAsync(string nia)
        {
            return await _context.Students.AnyAsync(s => s.Nia == nia);
        }

        public async Task<bool> DeleteAsyncPro(Student student)
        {
            try
            {
                var loadedStudent = await _context.Students
                    .Include(s => s.Incidents)
                    .FirstOrDefaultAsync(s => s.Nia == student.Nia);

                if (loadedStudent == null)
                    return false;

                int totalIncidents = loadedStudent.Incidents.Count;

                if (totalIncidents > 0)
                {
                    var confirm = MessageBox.Show(
                        $"This student is linked to {totalIncidents} incident(s).\nIf you continue, all related incidents will also be deleted.\n\nDo you want to proceed?",
                        "Confirm Deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (confirm != MessageBoxResult.Yes)
                        return false;
                }

                _context.Students.Remove(loadedStudent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}

