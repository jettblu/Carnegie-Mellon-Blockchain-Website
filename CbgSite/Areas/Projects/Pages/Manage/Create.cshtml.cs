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

namespace CbgSite.Areas.Projects.Pages.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateModel : PageModel
    {
        private Services.ProjectManager _projectManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;
        public CreateModel(Services.ProjectManager projectManager, CbgSiteContext contextCbg, UserManager<CbgUser> userManager,
            SignInManager<CbgUser> signInManager)
        {
            _projectManager = projectManager;
            _contextCbg = contextCbg;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public Data.Project Project { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchString { get; set; }
            public string SearchStringAsync { get; set; }
            public string Members { get; set; }
            // current selected members when user makes a query
            public string MembersOnQuery { get; set; }
        }
        public void OnGet()
        {
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var projectUser = new Data.ProjectUser()
            {
                DateCreated = DateTime.Now,
                CbgUserId = user.Id,
                ProjectId = Project.Id
            };
            _contextCbg.Projects.Add(Project);
            // add project creator to project team
            _contextCbg.ProjectUsers.Add(projectUser);

            await _contextCbg.SaveChangesAsync();
            var updateprojectUsersRes = await _projectManager.AddProjectUsersFromString(Input.Members, Project);

            if (updateprojectUsersRes != Globals.Status.Success) StatusMessage = "Unable to add project managers";
            return RedirectToPage("./Index");
        }

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


    }
}
