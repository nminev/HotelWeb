using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HotelWebsite
{
    public static class Utility
    {
        public static bool CheckIfImageExists(IWebHostEnvironment _env,string imageName)
        {
            string webRootPath = Path.Combine(_env.WebRootPath,"images");
            DirectoryInfo dInfo = new DirectoryInfo(webRootPath);
            var test = dInfo.GetFilesByExtensions(".jpg", "png");
            var boo= test.Any(x => x.Name == imageName);
            return boo;
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        private static string GetImageName(string imageSrc)
        {
            return imageSrc.Split("/").Last();
        }
    }
}
