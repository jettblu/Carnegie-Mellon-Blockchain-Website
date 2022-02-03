using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages
{
    public class IndexModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        private UserManager<CbgUser> _userManager { get; set; }
        public IndexModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg, UserManager<CbgUser> userManager)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
            _userManager = userManager;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        [BindProperty]
        public bool IsCreator { get; set; }
        [BindProperty]
        public List<CbgUser> ProjectUsers { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            Project = _contextCbg.Projects.FirstOrDefault(p => p.Id == id);
            ProjectUsers = await _projectManager.GetProjectUsers(Project);
            
            // toggle authorization based on custom parameters
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, Globals.Roles.SuperAdmin.ToString());
            if (isSuperAdmin || ProjectUsers.Contains(user))
            {
                IsCreator = true;
            }

            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
