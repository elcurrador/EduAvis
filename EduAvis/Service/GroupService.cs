using EduAvis.Backend.Model;

namespace EduAvis.Service
{
    public class GroupService : GenericService<Group>
    {
        private BdeduavisContext _context;
        public GroupService(BdeduavisContext context) : base(context)
        {
            _context = context;
        }
    }

}

