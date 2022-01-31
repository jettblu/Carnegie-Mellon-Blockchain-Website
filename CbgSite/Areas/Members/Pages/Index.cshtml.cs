using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CbgSite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Members.Pages
{
    public class IndexModel : PageModel
    {
        private CbgSiteContext _contextCbg { get; set; }
        public IndexModel(CbgSiteContext contextCbg)
        {
            _contextCbg = contextCbg;
        }
        [BindProperty]
        public Identity.Data.CbgUser Member { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member = _contextCbg.Users.FirstOrDefault(u => u.Id == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
