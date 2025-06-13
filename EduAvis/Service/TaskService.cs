using EduAvis.Resource.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace EduAvis.Service
{
    public static class TaskService
    {
        private static string GetFilePath(string dni) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EduAvis", $"tasks_{dni}.json");

        public static List<UserTask> LoadTasks(string dni)
        {
            try
            {
                string path = GetFilePath(dni);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(path))
                    return new List<UserTask>();

                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<UserTask>();

                var tasks = JsonConvert.DeserializeObject<List<UserTask>>(json) ?? new List<UserTask>();

       
                foreach (var task in tasks)
                {
                    if (string.IsNullOrEmpty(task.Dni))
                        task.Dni = dni;
                }

                return tasks;
            }
            catch (Exception ex)
            {
             
                System.Diagnostics.Debug.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<UserTask>();
            }
        }

        public static void SaveTasks(string dni, List<UserTask> tasks)
        {
            try
            {
                string path = GetFilePath(dni);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                foreach (var task in tasks)
                {
                    task.Dni = dni;
                }

                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
          
                System.Diagnostics.Debug.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }
    }
}