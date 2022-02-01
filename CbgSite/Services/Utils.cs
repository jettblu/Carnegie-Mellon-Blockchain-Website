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


        public static List<CbgUser> SearchUsers(CbgUser user, CbgSiteContext context, string query, int amount = 0)
        {
            var users = from m in context.Users
                        select m;

            // match username, username, or number based on query. Exclude current user from result
            return users.Where(s => (s.UserName.Contains(query) || s.PhoneNumber.StartsWith(query) || s.Name.Contains(query)) && s.UserName != user.UserName).ToList();
        }
    }
}
