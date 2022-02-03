using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Areas.Members.Data
{
    public class TagUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string TagId { get; set; }
        public string CbgUserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}
