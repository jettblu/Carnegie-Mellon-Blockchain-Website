using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Areas.Projects.Data
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        // name of project
        public string Name { get; set; }
        // photo that represents project
        public string MainPhotoPath { get; set; }
        // photo for quick, external display
        public string ThumbnailPath { get; set; }
        // custom color for project page
        public string Color { get; set; }
        // short description of project... used for quick display
        public string Description { get; set; }
        // extended description of project.. used on project page
        public string DescriptionLong { get; set; }
        // link to project's GitHub
        public string LinkGithub { get; set; }
        // link to project's twitter profile
        public string LinkTwitter { get; set; }

        public bool IsPublic { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}