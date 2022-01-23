using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CbgSite.Areas.Projects.Pages.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        public EditModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = _contextCbg.Projects.FirstOrDefault(p => p.Id == id);

            if (Project == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _contextCbg.Attach(Project).State = EntityState.Modified;
            try
            {
                await _contextCbg.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(Project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
/*        public IActionResult OnPostTestAsync(string id)
        {
            var test = Project;
            return RedirectToPage("./Index");
        }*/

        private bool ProjectExists(string id)
        {
            return _contextCbg.Projects.Any(e => e.Id == id);
        }
    }
}
