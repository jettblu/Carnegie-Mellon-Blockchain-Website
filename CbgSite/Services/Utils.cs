using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Services
{
    public class Utils
    {
        private static List<string> _ValidExtensions = new List<string>() { ".png", ".jpg", ".jpeg" };
        public static bool IsValidPhoto(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return _ValidExtensions.Contains(extension.ToLower());
        }


        
    }
}
