using System;
using System.IO;
using System.Text;

namespace Core.Utils
{
    public class ResourceUtils
    {
        private static readonly string FileLocation = "/Resources/Uploads";

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetPath(string fileName)
        {
            return new StringBuilder().Append(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName).Append(FileLocation).Append("/").Append(fileName).ToString();
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetRootPath(string fileName)
        {
            return new StringBuilder().Append(AppDomain.CurrentDomain.BaseDirectory).Append(fileName).ToString();
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                Directory.Delete(path);
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}