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
        public string LinkGithub { get; set; }
        public string LinkLinkedIn { get; set; }
        public string LinkTwitter { get; set; }
        public string AboutMe { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}
