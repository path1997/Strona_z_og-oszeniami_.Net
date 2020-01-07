using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia5.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public int AdId { get; set; }
        public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Ad Ad { get; set; }
    }
}