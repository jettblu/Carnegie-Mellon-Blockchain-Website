using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using CbgSite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace CbgSite.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;
        private TagManager _tagManager { get; set; }
        private CbgSiteContext _contextCbg { get; set; }

        public IndexModel(
            UserManager<CbgUser> userManager,
            SignInManager<CbgUser> signInManager,
            CbgSiteContext cbgSiteContext, 
            TagManager tagManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextCbg = cbgSiteContext;
            _tagManager = tagManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public List<Members.Data.Tag> Tags { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchString { get; set; }
            public string SearchStringAsync { get; set; }
            public string Tags { get; set; }
            // current selected members when user makes a query
            public string TagsOnQuery { get; set; }
            // project members on page load
            public string TagsOnLoad { get; set; }
        }

        [BindProperty]
        public CbgUser Member { get; set; }
        private async Task LoadAsync(CbgUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Member = user;
            /* Input = new InputModel
             {
                 PhoneNumber = phoneNumber,
                 Name = user.Name,
                 AboutMe = user.AboutMe,
                 LinkGitHub = user.LinkGithub,
                 LinkTwitter = user.LinkTwitter,
                 LinkLinkedIn = user.LinkLinkedIn
             };*/
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Tags = _tagManager.GetTagUsers(user);
            // build tag string for form 
            foreach (var t in Tags)
            {
                Input.TagsOnLoad = Input.TagsOnLoad + t.Id + ",";
            }
            
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateMemberAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // update basic profile info
            user.LinkGithub = Member.LinkGithub;
            user.LinkTwitter = Member.LinkTwitter;
            user.LinkLinkedIn = Member.LinkLinkedIn;
            user.AboutMe = Member.AboutMe;
            user.Name = Member.Name;
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update profile.";
                return RedirectToPage();
            }

            // uncomment below for input model update flow

            /*var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber || Input.Name != user.Name || Input.LinkGitHub != user.LinkGithub || 
                Input.LinkTwitter !=user.LinkTwitter 
                || Input.LinkLinkedIn != user.LinkLinkedIn || Input.AboutMe !=user.AboutMe)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                user.Name = Input.Name;
                var setNameResult = await _userManager.UpdateAsync(user);
                if (!setNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update profile.";
                    return RedirectToPage();
                }
            }*/


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostSearchTags()
        {

            var user = await _userManager.GetUserAsync(User);
            var query = Input.SearchStringAsync;


            // uncomment below to return page if search bypasses client side checks and is empty

            /*if (string.IsNullOrEmpty(query))
            {
                StatusMessage = "Please enter a valid query.";
                return Page();
            }*/

            var tags = Utils.SearchTags(user, _contextCbg, query, amount:5);
            // UNCOMMENT AND FIX TO filter results returned from search
            // match customer name, username, or number based on query
            /*            var tags = Utils.SearchTags(user, _contextCbg, query);
                        if (!string.IsNullOrEmpty(Input.TagsOnQuery))
                        {
                            // remove tags from search result if already selected
                            var tagIds = Input.TagsOnQuery.Split(",");
                            foreach (var tag in tagIds)
                            {
                                var tagToRemove = await _userManager.FindByIdAsync(tag.Id);
                                tags.Remove(tagToRemove);
                            }
                        }
                        // remove tags from search result if already in db with user
                        if (!string.IsNullOrEmpty(Input.TagsOnLoad))
                        {
                            // remove tags from search result if already selected
                            var projectMemberNames = Input.TagsOnLoad.Split(",");
                            foreach (var uname in projectMemberNames)
                            {
                                tags.Remove(tagToRemove);
                            }
                        }*/

            var searchModel = new SearchTagData()
            {
                IsNumber = false,
                Query = query,
                Tags = tags
            };

            return new PartialViewResult()
            {
                ViewName = "_tagSearchResult",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = searchModel
                }
            };
        }
        private bool MemberExists(string id)
        {
            return _contextCbg.Users.Any(e => e.Id == id);
        }
    }
}
