using CbgSite.Areas.Projects.Data;
using System.Collections.Generic;

namespace CbgSite.Services
{
    public interface IProjectManager
    {
        List<Project> GetProjects(string query);
    }
}