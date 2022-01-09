using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        public CreateModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
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

            _contextCbg.Projects.Add(Project);
            await _contextCbg.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
