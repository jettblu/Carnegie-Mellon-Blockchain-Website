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
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchString { get; set; }
            public string SearchStringAsync { get; set; }
            public string Members { get; set; }
        }
        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = _contextCbg.Projects.FirstOrDefault(p => p.Id == id);
            ProjectUsers = await _projectManager.GetProjectUsers(Project);
            // build project user string for form 
            /*string projectUserstring = "";
            foreach (var pu in ProjectUsers)
            {
                projectUserstring = projectUserstring + pu.UserName + ",";
            }
*/
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

            if(updateprojectUsersRes != Globals.Status.Success) StatusMessage = 

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
