using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Service
{
    public class RoleService : GenericService<Role>
    {
        private BdeduavisContext _context;
        public RoleService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByTeacherIdAsync(int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher == null || teacher.RoleId == null)
                return null;

            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == teacher.RoleId);
        }

    }

}

