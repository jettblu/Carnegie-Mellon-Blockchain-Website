using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite
{
    public class Globals
    {
        public enum Roles
        {
            SuperAdmin,
            Admin,
            Basic
        }
        public enum Status
        {
            Success = 0,
            Failure = 1,
            Pending = 2,
            Done = 3
        }
    }
}
