using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages
{
    public class IndexModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        public IndexModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        [BindProperty]
        public List<CbgUser> ProjectUsers { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = _contextCbg.Projects.FirstOrDefault(p => p.Id == id);
            ProjectUsers = await _projectManager.GetProjectUsers(Project);
            
            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
