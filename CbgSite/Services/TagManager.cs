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
        // return s all tags associated with user
        public List<Areas.Members.Data.Tag> GetUserTags(CbgUser user)
        {
            var tagUsers = _contextCbg.TagUsers.Where(p => p.CbgUserId == user.Id);
            List<Areas.Members.Data.Tag> resultTags = new List<Areas.Members.Data.Tag>();
            foreach (var ut in tagUsers)
            {
                var tag = _contextCbg.Tags.Where(t => t.Id == ut.TagId).FirstOrDefault();
                if (tag != null)
                {
                    resultTags.Add(tag);
                }
                
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
        // associates a tag with a given user... tagId is either a valid id or content to add
        public Areas.Members.Data.Tag AddTagUser(CbgUser user, string tagId)
        {
            Areas.Members.Data.Tag returnTag;
            var tagUserNew = new Areas.Members.Data.TagUser()
            {
                CbgUserId = user.Id,
                TagId = tagId,
                DateCreated = DateTime.Now
            };
            if (TagExists(tagId))
            {
                returnTag = GetTagById(tagId);
                // if this tag/user combo already exists, no need to add anything to db
                if (!TagUserExists(tagUserNew))
                {
                    _contextCbg.TagUsers.Add(tagUserNew);
                }
            }
            // if tag doesn't exist then taguser doesn't, so no need to case on it
            else
            {
                returnTag = new Areas.Members.Data.Tag()
                {
                    // content is string input if not in db
                    Content = tagId,
                    DateCreated = DateTime.Now
                };
               
                var resTagAdd = _contextCbg.Tags.Add(returnTag);
                // set tag user tag id to newly created tag id
                tagUserNew.TagId = resTagAdd.Entity.Id;
                _contextCbg.TagUsers.Add(tagUserNew);
            }
            _contextCbg.SaveChanges();

            return returnTag;
        }

        public void RemoveTagUserById(CbgUser user, string tagId)
        {
            var tagUser= new Areas.Members.Data.TagUser()
            {
                CbgUserId = user.Id,
                TagId = tagId,
                DateCreated = DateTime.Now
            };
            // can't remove tagUser if it doesn't exist!
            // catch errors in call function
            if (TagUserExists(tagUser))
            {
                var tagUserToRemove = GetTagUser(user, tagId);
                _contextCbg.TagUsers.Remove(tagUserToRemove);
                _contextCbg.SaveChanges();
            }
        }

        public Areas.Members.Data.Tag GetTagById(string tagId)
        {
            return _contextCbg.Tags.Where(t => t.Id == tagId).FirstOrDefault();
        }
        public Areas.Members.Data.TagUser GetTagUser(CbgUser user, string tagId)
        {
            return _contextCbg.TagUsers.Where(tu => tu.TagId == tagId && tu.CbgUserId == user.Id).FirstOrDefault();
        }

        private bool TagExists(string tagId)
        {
            return _contextCbg.Tags.Any(t => t.Id == tagId);
        }
        private bool TagUserExists(Areas.Members.Data.TagUser tagUser)
        {
            return _contextCbg.TagUsers.Any(t => t.CbgUserId == tagUser.CbgUserId && t.TagId == tagUser.TagId);
        }

    }
}
