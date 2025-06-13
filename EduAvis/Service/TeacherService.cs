using System.Windows;
using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EduAvis.Service
{
    public class TeacherService : GenericService<Teacher>
    {
        private readonly BdeduavisContext _context;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TeacherService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds teachers who are tutors of the given group
        /// </summary>
        /// <param name="groupName">The name of the group</param>
        /// <returns>A list of teachers</returns>
        public async Task<IEnumerable<Teacher>> FindTeacherByGroupAsync(string groupName)
        {
            try
            {
                var teachers = await _context.Teachers
                    .Where(t => t.TutorGroupNavigation.GroupName == groupName)
                    .ToListAsync();

                return teachers;
            }
            catch (Exception ex)
            {
                LogError("Error while fetching teachers by group", ex);
                return Enumerable.Empty<Teacher>();
            }
        }
        public async Task<Group?> GetGroupByTeacherAsync(Teacher teacher)
        {
            if (teacher == null || string.IsNullOrEmpty(teacher.TutorGroup))
                return null;

            return await _context.Groups
                .FirstOrDefaultAsync(g => g.GroupCode == teacher.TutorGroup);
        }
        private void LogError(string message, Exception ex)
        {
            logger.Error(message);
            logger.Error(ex.Message);
            logger.Error(ex.StackTrace);
        }

        public async Task<IEnumerable<Teacher>> getAllAsycAndRoles()
        {
            try
            {
                return await _context.Teachers
                    .Include(t => t.Role) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                LogError("Error while fetching all teachers with roles", ex);
                return Enumerable.Empty<Teacher>();
            }
        }
        // En TeacherService.cs
        public async Task<bool> HasMainTutorInGroupAsync(string groupCode)
        {
            try
            {
                return await _context.Teachers
                    .AnyAsync(t => t.TutorGroup == groupCode && t.IsTutor);
            }
            catch (Exception ex)
            {
                LogError("Error checking for existing main tutor", ex);
                return false;
            }
        }

        public async Task<bool> DniExistsAsync(string dni)
        {
            return await _context.Teachers.AnyAsync(t => t.Dni == dni);
        }

        public async Task<bool> DeleteAsyncPro(Teacher teacher)
        {
            try
            {
                // Cargar relaciones si no vienen incluidas
                var loadedTeacher = await _context.Teachers
                    .Include(t => t.IncidentRegisteredByNavigations)
                    .Include(t => t.IncidentTeacherDniNavigations)
                    .FirstOrDefaultAsync(t => t.Dni == teacher.Dni);

                if (loadedTeacher == null)
                    return false;

                int totalIncidents = loadedTeacher.IncidentRegisteredByNavigations.Count + loadedTeacher.IncidentTeacherDniNavigations.Count;

                if (totalIncidents > 0)
                {
                    var confirm = MessageBox.Show(
                        $"This teacher is linked to {totalIncidents} incident(s).\nIf you continue, all related incidents will also be deleted.\n\nDo you want to proceed?",
                        "Confirm Deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (confirm != MessageBoxResult.Yes)
                        return false;
                }

                _context.Teachers.Remove(loadedTeacher);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }




        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Teachers
                .AnyAsync(t => t.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email, string excludeTeacherDni)
        {
            return await _context.Teachers
                .AnyAsync(t => t.Email.ToLower() == email.ToLower() && t.Dni != excludeTeacherDni);
        }
    }
}
