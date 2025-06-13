using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Service
{
    public class IncidentService: GenericService<Incident>
    {
        private BdeduavisContext _context;
        public IncidentService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Incident>> GetAllAsync()
        {
            var query = _context.Incidents.AsQueryable();

            // Obtén todas las propiedades de navegación
            var navigationProperties = _context.Model.FindEntityType(typeof(Incident))
                .GetNavigations()
                .Select(e => e.Name);

            // Incluye cada propiedad de navegación
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return await query.ToListAsync();
        }

    



    }


}
