using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbgSite.Areas.Projects.Pages
{
    public class IndexModel : PageModel
    {
        List<Data.Project > Projects { get; set; }
        public void OnGet()
        {
        }
    }
}
