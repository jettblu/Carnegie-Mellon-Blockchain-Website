using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CbgSite.Areas.Members.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;

        public IndexModel(SignInManager<CbgUser> signInManager,
            UserManager<CbgUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IQueryable<CbgUser> Members { get; set; }
        public void OnGet()
        {
            // retrieve members and update model state
            Members = _userManager.Users;
        }
    }
}
