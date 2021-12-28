using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Services
{
    public class ProjectManager : IProjectManager
    {
        private readonly Data.CbgSiteContext _contextCbg;

        public ProjectManager(Data.CbgSiteContext contextCbg)
        {
            _contextCbg = contextCbg;
        }

        // retrieves all projects in descending order or projects that match query, if specified
        public List<Areas.Projects.Data.Project> GetProjects(string query = "")
        {
            if (String.IsNullOrEmpty(query))
            {
                return _contextCbg.Projects.OrderByDescending(p => p.DateCreated).ToList();
            }
            else
            {
                return _contextCbg.Projects.Where(p => p.Name.Contains(query)).OrderByDescending(p => p.DateCreated).ToList();
            }
        }
    }
}
