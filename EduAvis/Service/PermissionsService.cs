using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Service
{
    public class PermissionsService : GenericService<Permission>
    {
        private BdeduavisContext _context;
        public PermissionsService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task UpdateRolePermissionsAsync(int roleId, List<Permission> newPermissions)
        {
            var existing = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
            _context.RolePermissions.RemoveRange(existing);

            foreach (var permission in newPermissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.Id
                });
            }

            await _context.SaveChangesAsync();
        }
    }

}

