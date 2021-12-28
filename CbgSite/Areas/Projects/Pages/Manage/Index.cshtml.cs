using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        public IndexModel(Services.ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        public List<Data.Project> Projects { get; set; }
        public void OnGet()
        {
            Projects = _projectManager.GetProjects();
        }
    }
}
