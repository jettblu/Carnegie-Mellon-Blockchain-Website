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
            var projectUsers =  _contextCbg.ProjectUsers.Where(p => p.ProjectId == project.Id).ToList();
            List<CbgUser> resultUsers = new List<CbgUser>();
            foreach (var pu in projectUsers)
            {
                var user = await _userManager.FindByIdAsync(pu.CbgUserId);
                resultUsers.Add(user);
            }
            return resultUsers;
        }

        public async Task<Globals.Status> AddProjectUsersFromString(string userString, Areas.Projects.Data.Project project)
        {
            var projectMemberNames = userString.Split(",");
            List<CbgUser> projectUsers = new List<CbgUser>();
            foreach (var uname in projectMemberNames)
            {
                var user = await _userManager.FindByNameAsync(uname);
                projectUsers.Add(user);
                var projectUserNew = new Areas.Projects.Data.ProjectUser()
                {
                    ProjectId = project.Id,
                    CbgUserId = user.Id
                };
                if (!ProjectUserExists(projectUserNew))
                {
                    _contextCbg.ProjectUsers.Add(projectUserNew);
                }
            }

            _contextCbg.SaveChanges();
            return Globals.Status.Success;
        }

        public List<CbgUser> SearchUsers(CbgUser user, string query, int amount = 0)
        {
            // return top n matches where n=amount parameter
            if (amount != 0)
            {
                // match username, username, or number based on query. Exclude current user from result
                return _contextCbg.Users.Where(s => (s != user && s.UserName.Contains(query) || s.PhoneNumber.StartsWith(query) || s.Name.Contains(query)
                || s.Email.Contains(query))).Take(amount).ToList();
            }
            else
            {
                // match username, username, or number based on query. Exclude current user from result
                return _contextCbg.Users.Where(s => (s != user && s.UserName.Contains(query) || s.PhoneNumber.StartsWith(query) || s.Name.Contains(query)
                || s.Email.Contains(query))).ToList();
            }

        }

        private bool ProjectUserExists(Areas.Projects.Data.ProjectUser projectUser)
        {
            return _contextCbg.ProjectUsers.Any(e => e.ProjectId == projectUser.ProjectId && e.CbgUserId == projectUser.CbgUserId);
        }
    }
}
