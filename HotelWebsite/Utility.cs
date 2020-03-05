using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite
{
    public static class Utility
    {
        public static bool CheckIfImageExists(string imagePath)
        {
            return File.Exists("~/images/" +imagePath);
        }

        private static string GetImageName(string imageSrc)
        {
            return imageSrc.Split("/").Last();
        }
    }
}
