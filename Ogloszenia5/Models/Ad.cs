using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia5.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}