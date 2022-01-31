using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CbgSite.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private readonly SignInManager<CbgUser> _signInManager;
        private CbgSiteContext _contextCbg { get; set; }

        public IndexModel(
            UserManager<CbgUser> userManager,
            SignInManager<CbgUser> signInManager,
            CbgSiteContext cbgSiteContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextCbg = cbgSiteContext;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

/*        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "About Me")]
            public string AboutMe { get; set; }
            [Display(Name = "Link Github")]
            public string LinkGitHub { get; set; }
            [Display(Name = "Link Twitter")]
            public string LinkTwitter { get; set; }
            [Display(Name = "Link LinkedIn")]
            public string LinkLinkedIn { get; set; }
        }*/

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
        private bool MemberExists(string id)
        {
            return _contextCbg.Users.Any(e => e.Id == id);
        }
    }
}
