using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia5.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public int AdId { get; set; }
        public string Url { get; set; }
        public int position { get; set; }
        public virtual Ad Ad { get; set; }
    }
}