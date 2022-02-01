using CbgSite.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Data
{
    public class SearchData
    {
        public string Query;
        public bool IsNumber;
        public List<CbgUser> Users;
    }
}
