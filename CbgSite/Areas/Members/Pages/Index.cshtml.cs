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
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public List<CbgUser> Members { get; set; }
        [BindProperty]
        public Globals.Roles MemberRole{ get; set; }
        [BindProperty]
        public string MemberId { get; set; }
        public void OnGet()
        {
            // retrieve members and update model state          
            Members = _contextCbg.Users.ToList();
        }
        // ADD ABILITY TO REMOVE FROM ALL ROLES (or display multiple at same time)
        public async Task<JsonResult> OnPostUpdateRolesAsync()
        {
            var user = await _userManager.FindByIdAsync(MemberId);
            // remove all roles from user
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            // add user to new role
            var res = await _userManager.AddToRoleAsync(user, MemberRole.ToString());
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Member Role Updated!";
            return new JsonResult("Role function completed");
        }
    }
}
