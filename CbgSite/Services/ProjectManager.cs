using CbgSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Services
{
    public class ProjectManager : IProjectManager
    {
        private readonly Data.CbgSiteContext _contextCbg;
        private readonly UserManager<CbgUser> _userManager;
        public ProjectManager(Data.CbgSiteContext contextCbg, UserManager<CbgUser> userManager)
        {
            _contextCbg = contextCbg;
            _userManager = userManager;
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

        public async Task<List<CbgUser>> GetProjectUsers(Areas.Projects.Data.Project project)
        {
            var projectUsers =  _contextCbg.ProjectUsers.Where(p => p.ProjectId == project.Id);
            List<CbgUser> resultUsers = new List<Areas.Identity.Data.CbgUser>();
            foreach (var pu in projectUsers)
            {
                var user = await _userManager.FindByIdAsync(pu.CbgUserId);
                resultUsers.Add(user);
            }
            return resultUsers;
        }
    }
}
