using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages
{
    public class AllModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        public AllModel(Services.ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        List<Data.Project> Projects { get; set; }
        public void OnGet()
        {
            Projects = _projectManager.GetProjects();
        }
    }
}
