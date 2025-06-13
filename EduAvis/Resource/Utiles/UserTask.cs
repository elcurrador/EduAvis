using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAvis.Resource.Utiles
{
    public class UserTask
    {
        public string Dni { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public bool IsCompleted { get; set; }

        public UserTask()
        {
            Dni = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            IsCompleted = false;
        }
    }
}