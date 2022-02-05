using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Data
{
    public class SearchTagData
    {
        public string Query;
        public bool IsNumber;
        public List<Areas.Members.Data.Tag> Tags;
    }
}
