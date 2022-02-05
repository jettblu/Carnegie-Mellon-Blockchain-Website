using CbgSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Services
{
    public class TagManager
    {
        private readonly Data.CbgSiteContext _contextCbg;
        private readonly UserManager<CbgUser> _userManager;
        public TagManager(Data.CbgSiteContext contextCbg, UserManager<CbgUser> userManager)
        {
            _contextCbg = contextCbg;
            _userManager = userManager;
        }
        public List<Areas.Members.Data.Tag> GetTagUsers(CbgUser user)
        {
            var tagUsers = _contextCbg.TagUsers.Where(p => p.CbgUserId == user.Id);
            List<Areas.Members.Data.Tag> resultTags = new List<Areas.Members.Data.Tag>();
            foreach (var ut in tagUsers)
            {
                var tag = _contextCbg.Tags.Where(t => t.Id == ut.TagId).First();
                resultTags.Add(tag);
            }
            return resultTags;
        }

        public async Task<Globals.Status> AddTagUserFromString(string tagString, CbgUser user)
        {
            var tagIds = tagString.Split(",");
            List<CbgUser> tagUsers = new List<CbgUser>();
            foreach (var tId in tagIds)
            {
                if(!_contextCbg.Tags.Any(t => t.Id == tId))
                {
                    var tagNew = new Areas.Members.Data.Tag()
                    {
                        // content is string input if not in db
                        Content = tId,
                        DateCreated = DateTime.Now
                    };
                    _contextCbg.Tags.Add(tagNew);
                }

                var tagUserNew = new Areas.Members.Data.TagUser()
                {

                };
                
                if (!TagUserExists(tagUserNew))
                {
                    _contextCbg.TagUsers.Add(tagUserNew);
                }
            }

            _contextCbg.SaveChanges();
            return Globals.Status.Success;
        }
        private bool TagUserExists(Areas.Members.Data.TagUser tagUser)
        {
            return _contextCbg.TagUsers.Any(t => t.CbgUserId == tagUser.CbgUserId && t.TagId == tagUser.TagId);
        }

    }
}
