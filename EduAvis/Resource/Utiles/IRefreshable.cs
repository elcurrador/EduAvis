using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAvis.Resource.Utiles
{
    public interface IRefreshable
    {
        Task Refresh();
    }
}
