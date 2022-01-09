using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CbgSite.Areas.Members.Pages
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;
        private CbgSiteContext _contextCbg;

        public IndexModel(SignInManager<CbgUser> signInManager,
            UserManager<CbgUser> userManager, CbgSiteContext cbgSiteContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextCbg = cbgSiteContext;
        }
        public List<CbgUser> Members { get; set; }
        public void OnGet()
        {
            // retrieve members and update model state          
            Members = _contextCbg.Users.ToList();
        }
    }
}
