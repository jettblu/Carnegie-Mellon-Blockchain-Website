using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using CbgSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace CbgSite.Areas.Projects.Pages.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        private readonly UserManager<CbgUser> _userManager;
        public EditModel(ProjectManager projectManager, CbgSiteContext contextCbg, UserManager<CbgUser> userManager)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
            _userManager = userManager;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        [BindProperty]
        public List<CbgUser> ProjectUsers { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public bool IsAuthorized { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            public string SearchString { get; set; }
            public string SearchStringAsync { get; set; }
            public string Members { get; set; }
            // current selected members when user makes a query
            public string MembersOnQuery { get; set; }
            // project members on page load
            public string MembersOnLoad { get; set; }
        }
        public async Task<IActionResult> OnGet(string id)
        {
            IsAuthorized = false;
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            Project = _contextCbg.Projects.FirstOrDefault(p => p.Id == id);
            ProjectUsers = await _projectManager.GetProjectUsers(Project);
            // toggle authorization based on custom parameters
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, Globals.Roles.SuperAdmin.ToString());
            if (isSuperAdmin || ProjectUsers.Contains(user))
            {
                IsAuthorized = true;
            }
            Input = new InputModel();
            
            // build project user string for form 
            foreach (var pu in ProjectUsers)
            {
                Input.MembersOnLoad = Input.MembersOnLoad + pu.UserName + ",";
            }

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

            var updateprojectUsersRes = await _projectManager.AddProjectUsersFromString(Input.Members, Project);

            if (updateprojectUsersRes != Globals.Status.Success) StatusMessage = "Unable to add project managers";

            return RedirectToPage("./Index");
        }
        /*        public IActionResult OnPostTestAsync(string id)
                {
                    var test = Project;
                    return RedirectToPage("./Index");
                }*/

        public async Task<IActionResult> OnPostSearchUsers()
        {

            var user = await _userManager.GetUserAsync(User);
            var query = Input.SearchStringAsync;


            // uncomment below to return page if search bypasses client side checks and is empty

            /*if (string.IsNullOrEmpty(query))
            {
                StatusMessage = "Please enter a valid query.";
                return Page();
            }*/



            // match customer name, username, or number based on query
            var users = Utils.SearchUsers(user, _contextCbg, query);
            if (!string.IsNullOrEmpty(Input.MembersOnQuery))
            {
                // remove users from search result if already selected
                var projectMemberNames = Input.MembersOnQuery.Split(",");
                foreach (var uname in projectMemberNames)
                {
                    var userToRemove = await _userManager.FindByNameAsync(uname);
                    users.Remove(userToRemove);
                }
            }
            // remove users from search result if already in db with project
            if (!string.IsNullOrEmpty(Input.MembersOnLoad))
            {
                // remove users from search result if already selected
                var projectMemberNames = Input.MembersOnLoad.Split(",");
                foreach (var uname in projectMemberNames)
                {
                    var userToRemove = await _userManager.FindByNameAsync(uname);
                    users.Remove(userToRemove);
                }
            }

            var searchModel = new SearchData()
            {
                IsNumber = false,
                Query = query,
                Users = users
            };

            return new PartialViewResult()
            {
                ViewName = "_userSearchResult",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = searchModel
                }
            };
        }

        private bool ProjectExists(string id)
        {
            return _contextCbg.Projects.Any(e => e.Id == id);
        }
    }
}
