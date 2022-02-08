using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using CbgSite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Members.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CbgUser> _userManager;
        private CbgSiteContext _contextCbg { get; set; }
        private TagManager _tagManager { get; set; }
        public IndexModel(CbgSiteContext contextCbg, TagManager tagManager, UserManager<CbgUser> userManager)
        {
            _contextCbg = contextCbg;
            _tagManager = tagManager;
            _userManager = userManager;
        }
        [BindProperty]
        public Identity.Data.CbgUser Member { get; set; }
        public List<Members.Data.Tag> Tags { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            if(!MemberExists(id)) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            Member = _contextCbg.Users.FirstOrDefault(u => u.Id == id);
            Tags = _tagManager.GetUserTags(Member);
            try
            {
                var x = "";   
            }
            catch
            {
                Tags = new List<Data.Tag>();
                Tags.Add(new Data.Tag()
                {
                    Id = "na",
                    Content = "DeFi"
                });
            }
            
            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
        private bool MemberExists(string id)
        {
            return _contextCbg.Users.Any(e => e.Id == id);
        }
    }
}
