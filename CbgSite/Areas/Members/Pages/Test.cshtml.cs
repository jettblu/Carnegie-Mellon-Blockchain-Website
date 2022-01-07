using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Members.Pages
{
    [Authorize(Roles = "SuperAdmin")]
    public class TestModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;

        public TestModel(SignInManager<CbgUser> signInManager,
            UserManager<CbgUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {

            public CbgUser UserCurrent { get; set; }
        }
        public async void OnGet()
        {
        }

        public async Task OnPostAddRole()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = Globals.Roles.SuperAdmin.ToString();
            var res = await _userManager.AddToRoleAsync(user, role);
        }
    }
}
