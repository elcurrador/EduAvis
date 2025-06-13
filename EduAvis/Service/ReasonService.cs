using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Service
{
    public class ReasonService : GenericService<Reason>
    {
        private BdeduavisContext _context;
        public ReasonService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }

      
    }

}

