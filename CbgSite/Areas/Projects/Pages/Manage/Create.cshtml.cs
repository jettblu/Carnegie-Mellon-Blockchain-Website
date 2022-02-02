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

namespace CbgSite.Areas.Projects.Pages.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;
        public CreateModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg, UserManager<CbgUser> userManager,
            SignInManager<CbgUser> signInManager)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchString { get; set; }
            public string SearchStringAsync { get; set; }
            public string Members { get; set; }
        }
        public void OnGet()
        {
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var projectUser = new Data.ProjectUser()
            {
                DateCreated = DateTime.Now,
                CbgUserId = user.Id,
                ProjectId = Project.Id
            };
            _contextCbg.Projects.Add(Project);
            // add project creator to project team
            _contextCbg.ProjectUsers.Add(projectUser);
            await _contextCbg.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
