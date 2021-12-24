using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CrypticPayUser class
    public class CbgUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string ProfilePhotoPath { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}
